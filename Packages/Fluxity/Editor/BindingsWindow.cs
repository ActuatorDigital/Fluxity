using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using System.Linq;
using System.Reflection;

namespace AIR.Fluxity.Editor
{
    internal class BindingsWindow : FluxityRuntimeEditorWindow
    {
        private readonly Color _lighterColor = Color.white * 0.3f;
        private readonly Color _darkerColor = Color.white * 0.1f;

        //Invaluable help from https://gamedev.stackexchange.com/questions/188771/creating-a-custom-editor-window-using-a-multi-column-header
        private Vector2 _scrollViewPos;
        private MultiColumnHeaderState _multiColumnHeaderState;
        private MultiColumnHeader _multiColumnHeader;
        private PropertyInfo[] _dataColumnMapping;
        private Action<object, Rect>[] _dataColumnDrawers;
        private MultiColumnHeaderState.Column[] _columns;
        private List<Binding> _cachedBindings = new List<Binding>();


        [MenuItem("Window/Fluxity/Runtime Bindings")]
        public static void ShowWindow()
        {
            // Get existing open window or if none, make a new one:
            var window = GetWindow<BindingsWindow>();
            window.titleContent = new GUIContent("Fluxity Runtime Bindings");
            window.Show();
        }

        public void OnGUI()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                GUILayout.Label("Not in play mode.");
            }
            else
            {
                CalculateBindings();
                DrawBindings();
            }
        }

        protected override void Refresh()
        {
            base.Refresh();
            _cachedBindings.Clear();
            Initialize();
        }

        private void Initialize()
        {
            // include internals as we are the thing messing with it
            _dataColumnMapping = typeof(Binding).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            _dataColumnDrawers = new Action<object, Rect>[]
            {
                (x, r) => EditorGUI.LabelField(r, ((Binding.BindingElementType)x).ToString()),
                (x, r) => EditorGUI.LabelField(r, ((Type)x)?.Name ?? "N/A"),
                (x, r) => EditorGUI.LabelField(r, ((Type)x)?.Name ?? "N/A"),
                (x, r) => EditorGUI.LabelField(r, $"{((MethodInfo)x).DeclaringType.Name}.{((MethodInfo)x).Name}"),
            };

            _columns = _dataColumnMapping
                .Select((x, i) => new MultiColumnHeaderState.Column { headerContent = new GUIContent(_dataColumnMapping[i].Name) })
                .ToArray();

            _multiColumnHeaderState = new MultiColumnHeaderState(columns: _columns);
            _multiColumnHeader = new MultiColumnHeader(state: _multiColumnHeaderState);
            _multiColumnHeader.visibleColumnsChanged += (multiColumnHeader) => multiColumnHeader.ResizeToFit();
            _multiColumnHeader.sortingChanged += (multiColumnHeader) => { SortData(multiColumnHeader); Repaint(); };
            _multiColumnHeader.ResizeToFit();
        }

        private void DrawBindings()
        {
            GUILayout.BeginArea(new Rect(0, 0, position.width, position.height));
            {
                _scrollViewPos = EditorGUILayout.BeginScrollView(
                    _scrollViewPos,
                    GUILayout.Height(position.height),
                    GUILayout.Width(position.width));
                {
                    if (GUILayout.Button("Refresh"))
                    {
                        Refresh();
                    }

                    var store = GetStore();
                    if (store == null)
                    {
                        EditorGUILayout.LabelField("No store found");
                    }
                    else
                    {
                        if (_multiColumnHeader == null)
                        {
                            Initialize();
                        }

                        GUILayout.FlexibleSpace();
                        var windowRect = GUILayoutUtility.GetLastRect();

                        windowRect.width = position.width;
                        windowRect.height = position.height;
                        var columnHeight = EditorGUIUtility.singleLineHeight;

                        var columnRectPrototype = new Rect(windowRect)
                        {
                            height = columnHeight,
                        };

                        _multiColumnHeader.OnGUI(columnRectPrototype, 0.0f);

                        for (int i = 0; i < _cachedBindings.Count; i++)
                        {
                            var item = _cachedBindings[i];
                            var rowRect = new Rect(columnRectPrototype);

                            rowRect.y += columnHeight * (i + 1);

                            EditorGUI.DrawRect(rowRect, i % 2 == 0 ? _darkerColor : _lighterColor);

                            for (int columnIndex = 0; columnIndex < _dataColumnMapping.Length; columnIndex++)
                            {
                                if (!_multiColumnHeader.IsColumnVisible(columnIndex)) continue;

                                var visibleColumnIndex = _multiColumnHeader.GetVisibleColumnIndex(columnIndex);
                                var columnRect = _multiColumnHeader.GetColumnRect(visibleColumnIndex);
                                columnRect.y = rowRect.y;

                                var cellRect = _multiColumnHeader.GetCellRect(visibleColumnIndex, columnRect);
                                var objectValue = _dataColumnMapping[columnIndex].GetValue(item);
                                _dataColumnDrawers[columnIndex].Invoke(objectValue, cellRect);
                            }
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        private void SortData(MultiColumnHeader multiColumnHeader)
        {
            var columnIndex = multiColumnHeader.sortedColumnIndex;
            var isAscending = multiColumnHeader.IsSortedAscending(columnIndex);
            var propertyInfo = _dataColumnMapping[columnIndex];
            Func<Binding, object> sortFunction = x => propertyInfo.GetValue(x)?.ToString() ?? "N/A";

            _cachedBindings = isAscending
                ? _cachedBindings.OrderBy(sortFunction).ToList()
                : _cachedBindings.OrderByDescending(sortFunction).ToList();
        }

        private void CalculateBindings()
        {
            if (_cachedBindings.Count > 0)
                return; // already calculated

            var store = GetStore();
            if (store == null)
                return;

            var features = store.GetAllFeatures();
            _cachedBindings = new List<Binding>();
            foreach (var feat in features)
            {
                var commandsHandled = feat.GetAllHandledCommandTypes();

                foreach (var commandType in commandsHandled)
                {
                    var handlers = feat.GetAllReducersForCommand(commandType);

                    foreach (var handler in handlers)
                    {
                        var binding = new Binding()
                        {
                            BindingType = Binding.BindingElementType.Reducer,
                            Command = handler.CommandType,
                            Feature = feat.GetType().GenericTypeArguments[0],
                            Handler = handler.ReducerBindingInfo(),
                        };

                        _cachedBindings.Add(binding);
                    }
                }
            }

            var dispatcher = GetDispatcher();
            if (dispatcher == null)
                return;

            var allCommandsTypes = dispatcher.GetAllEffectCommandTypes();

            foreach (var commandType in allCommandsTypes)
            {
                var allEffects = dispatcher.GetAllEffectsForCommandType(commandType);
                foreach (var effect in allEffects)
                {
                    var binding = new Binding()
                    {
                        BindingType = Binding.BindingElementType.Effect,
                        Command = commandType,
                        Feature = null,
                        Handler = effect.EffectBindingInfo(),
                    };

                    _cachedBindings.Add(binding);
                }
            }
        }

        internal class Binding
        {
            internal enum BindingElementType
            {
                Reducer,
                Effect,
            }

            internal BindingElementType BindingType { get; set; }
            internal Type Command { get; set; }
            internal Type Feature { get; set; }
            internal MethodInfo Handler { get; set; }
        }
    }
}