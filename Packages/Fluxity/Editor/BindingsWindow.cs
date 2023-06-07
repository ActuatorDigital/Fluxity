using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AIR.Fluxity.Editor
{
    internal class BindingsWindow : FluxityRuntimeEditorWindow
    {
        private const string NA_STRING = "N/A";
        
        private PropertyInfo[] _dataColumnMapping;
        private Func<object, string>[] _dataMapperStringifiers;
        private List<Binding> _cachedBindings = new List<Binding>();
        private SearchBoxUtility _searchBoxUtility;
        private MultiColumnUtility _multiColumnUtility;

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

            var columnsNames = _dataColumnMapping.Select(x => x.Name).ToArray();

            _multiColumnUtility = new MultiColumnUtility();
            _multiColumnUtility.Initialise(
                columnsNames,
                (i, b) => { SortData(i,b); Repaint(); });

            _searchBoxUtility = new SearchBoxUtility();
            _searchBoxUtility.OnSearchTextChanged += UpdateFilteredResults;
        }

        private void DrawBindings()
        {
            var totalArea = new Rect(0, 0, position.width, position.height);
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
                    if (_multiColumnUtility == null)
                    {
                        Initialize();
                    }

                    _searchBoxUtility.OnGui();

                    GUILayout.FlexibleSpace();
                    var windowRect = GUILayoutUtility.GetLastRect();
                    windowRect.width = position.width;
                    windowRect.height = position.height - EditorGUIUtility.singleLineHeight;

                    _multiColumnUtility.OnGui(
                        windowRect,
                        _cachedBindings.Count,
                        i => _cachedBindings[i].MeetsFilter,
                        (itemIndex, columnIndex) =>
                        {
                            var item = _cachedBindings[itemIndex];
                            var objectValue = _dataColumnMapping[columnIndex].GetValue(item);
                            var strToShow = _dataMapperStringifiers[columnIndex].Invoke(objectValue);
                            return strToShow;
                        });
                }
            }
            GUILayout.EndArea();
        }

        private void UpdateFilteredResults(string searchText)
        {
            foreach (var item in _cachedBindings)
            {
                item.MeetsFilter = false;

                for (int columnIndex = 0; columnIndex < _dataColumnMapping.Length; columnIndex++)
                {
                    if (!_multiColumnUtility.IsColumnVisible(columnIndex)) continue;

                    var objectValue = _dataColumnMapping[columnIndex].GetValue(item);
                    var strToShow = _dataMapperStringifiers[columnIndex].Invoke(objectValue);
                    if (strToShow.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        item.MeetsFilter = true;
                }
            }
        }

        private void SortData(int columnIndex, bool isAscending)
        {
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