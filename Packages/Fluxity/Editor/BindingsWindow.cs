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
        private const string NA_STRING = "N/A";
        private readonly Color _lighterColor = Color.white * 0.3f;
        private readonly Color _darkerColor = Color.white * 0.1f;

        //Invaluable help from https://gamedev.stackexchange.com/questions/188771/creating-a-custom-editor-window-using-a-multi-column-header
        private Vector2 _scrollViewPos;
        private MultiColumnHeaderState _multiColumnHeaderState;
        private MultiColumnHeader _multiColumnHeader;
        private PropertyInfo[] _dataColumnMapping;
        private Func<object, string>[] _dataMapperStringifiers;
        private MultiColumnHeaderState.Column[] _columns;
        private List<Binding> _cachedBindings = new List<Binding>();
        private Vector2 _scrollPosition;
        private SearchField _searchField;
        private string _searchText = string.Empty;

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
            _dataColumnMapping = typeof(Binding).GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
            _dataMapperStringifiers = new Func<object, string>[]
            {
                (x) => ((Binding.BindingElementType)x).ToString(),
                (x) => ((Type)x)?.Name ?? NA_STRING,
                (x) => ((Type)x)?.Name ?? NA_STRING,
                (x) => $"{((MethodInfo)x).DeclaringType.Name}.{((MethodInfo)x).Name}",
            };

            _columns = _dataColumnMapping
                .Select((x, i) => new MultiColumnHeaderState.Column { headerContent = new GUIContent(_dataColumnMapping[i].Name) })
                .ToArray();

            _multiColumnHeaderState = new MultiColumnHeaderState(columns: _columns);
            _multiColumnHeader = new MultiColumnHeader(state: _multiColumnHeaderState);
            _multiColumnHeader.visibleColumnsChanged += (multiColumnHeader) => multiColumnHeader.ResizeToFit();
            _multiColumnHeader.sortingChanged += (multiColumnHeader) => { SortData(multiColumnHeader); Repaint(); };
            _multiColumnHeader.ResizeToFit();

            _searchField = new SearchField();
        }

        private void DrawBindings()
        {
            var pad = 0;
            var totalArea = new Rect(pad, pad, position.width-pad*2, position.height - pad * 2);
            GUILayout.BeginArea(totalArea);
            {
                var store = GetStore();
                if (store == null)
                {
                    if (GUILayout.Button("Refresh"))
                    {
                        Refresh();
                    }

                    EditorGUILayout.LabelField("No store found");
                }
                else
                {
                    if (_multiColumnHeader == null)
                    {
                        Initialize();
                    }

                    var prevSearchText = _searchText;
                    _searchText = _searchField.OnGUI(_searchText);
                    if (_searchText != prevSearchText)
                        UpdateFilteredResults();
                    
                    GUILayout.FlexibleSpace();
                    var windowRect = GUILayoutUtility.GetLastRect();

                    windowRect.width = position.width;
                    windowRect.height = position.height - EditorGUIUtility.singleLineHeight;
                    var columnHeight = EditorGUIUtility.singleLineHeight;

                    var columnRectPrototype = new Rect(windowRect)
                    {
                        height = columnHeight,
                    };

                    var viewRect = new Rect(source: windowRect)
                    {
                        width = _multiColumnHeaderState.widthOfAllVisibleColumns,
                        height = columnHeight * (_cachedBindings.Count + 1),
                    };

                    _scrollPosition = GUI.BeginScrollView(
                        windowRect,
                        scrollPosition: _scrollPosition,
                        viewRect,
                        true,
                        true
                    );
                    {
                        _multiColumnHeader.OnGUI(columnRectPrototype, 0.0f);
                        var rowRect = new Rect(columnRectPrototype);
                        int drawnCount = 0;

                        for (int i = 0; i < _cachedBindings.Count; i++)
                        {
                            var item = _cachedBindings[i];
                            if (!item.MeetsFilter)
                                continue;

                            rowRect.y += columnHeight;

                            EditorGUI.DrawRect(rowRect, drawnCount % 2 == 0 ? _darkerColor : _lighterColor);
                            drawnCount++;

                            for (int columnIndex = 0; columnIndex < _dataColumnMapping.Length; columnIndex++)
                            {
                                if (!_multiColumnHeader.IsColumnVisible(columnIndex)) continue;

                                var visibleColumnIndex = _multiColumnHeader.GetVisibleColumnIndex(columnIndex);
                                var objectValue = _dataColumnMapping[columnIndex].GetValue(item);
                                var strToShow = _dataMapperStringifiers[columnIndex].Invoke(objectValue);

                                var columnRect = _multiColumnHeader.GetColumnRect(visibleColumnIndex);
                                columnRect.y = rowRect.y;
                                var cellRect = _multiColumnHeader.GetCellRect(visibleColumnIndex, columnRect);
                                EditorGUI.LabelField(cellRect, strToShow);
                            }
                        }
                    }
                    GUI.EndScrollView(true);
                }
            }
            GUILayout.EndArea();
        }

        private void UpdateFilteredResults()
        {
            foreach (var item in _cachedBindings)
            {
                item.MeetsFilter = false;
                
                for (int columnIndex = 0; columnIndex < _dataColumnMapping.Length; columnIndex++)
                {
                    if (!_multiColumnHeader.IsColumnVisible(columnIndex)) continue;

                    var visibleColumnIndex = _multiColumnHeader.GetVisibleColumnIndex(columnIndex);
                    var objectValue = _dataColumnMapping[columnIndex].GetValue(item);
                    var strToShow = _dataMapperStringifiers[columnIndex].Invoke(objectValue);
                    if (strToShow.Contains(_searchText, StringComparison.OrdinalIgnoreCase))
                        item.MeetsFilter = true;
                }
            }
        }

        private void SortData(MultiColumnHeader multiColumnHeader)
        {
            var columnIndex = multiColumnHeader.sortedColumnIndex;
            var isAscending = multiColumnHeader.IsSortedAscending(columnIndex);
            var propertyInfo = _dataColumnMapping[columnIndex];
            Func<Binding, object> sortFunction = x => propertyInfo.GetValue(x)?.ToString() ?? NA_STRING;

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

        internal class Filterable
        {
            internal bool MeetsFilter { get; set; } = true;
        }
        
        internal class Binding : Filterable
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