using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace AIR.Fluxity.Editor
{
    internal class StoreWindow : FluxityRuntimeEditorWindow
    {
        private const int LeftPanelRightWall = 220;
        
        private string _currentlyShowingName;
        private FeatureStateDrawer _featureDrawer;
        private SearchBoxUtility _storeSearchBox;
        private TwoPanelUtility _twoPanel;

        [MenuItem("Window/Fluxity/Runtime Stores")]
        public static void ShowWindow()
        {
            // Get existing open window or if none, make a new one:
            var window = GetWindow<StoreWindow>();
            window.titleContent = new GUIContent("Fluxity Runtime Stores");
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
                if (_twoPanel == null)
                    Refresh();

                _twoPanel.OnGui(position);
            }
        }

        protected override void Refresh()
        {
            base.Refresh();
            _currentlyShowingName = null;
            _featureDrawer = null;
            _storeSearchBox = new SearchBoxUtility();
            _twoPanel = new TwoPanelUtility(
                LeftPanelRightWall,
                DoSelectStore,
                DoShowStore);
        }

        private void DoSelectStore()
        {
            if (GUILayout.Button("Clear Selection"))
            {
                _currentlyShowingName = string.Empty;
            }

            var store = GetStore();
            if (store == null)
            {
                EditorGUILayout.LabelField("No store found");
            }
            else
            {
                _storeSearchBox?.OnGui();
                EditorGUILayout.LabelField("Stores", EditorStyles.boldLabel);
                EditorWindowUtil.DrawHorLine(Color.grey);
                var filter = _storeSearchBox?.CurrentSearchText ?? string.Empty;
                foreach (var feat in store.GetAllFeatures())
                {
                    var name = feat.GetType().GenericTypeArguments[0].Name;
                    if (!name.Contains(filter, StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (GUILayout.Button(
                        name,
                        _currentlyShowingName == name ? EditorStyles.selectionRect : EditorStyles.label,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                    {
                        _currentlyShowingName = name;
                    }
                }
            }
        }

        private void DoShowStore()
        {
            var drawer = GetObjectDrawerForTarget();

            if (drawer == null)
            {
                EditorGUILayout.LabelField("No feature selected");
            }
            else
            {
                EditorGUILayout.LabelField($"Values of {_currentlyShowingName}", EditorStyles.boldLabel);
                EditorWindowUtil.DrawHorLine(Color.grey);

                drawer.Draw();
            }
        }

        private FeatureStateDrawer GetObjectDrawerForTarget()
        {
            var targetFeat = GetStore()?.GetAllFeatures()
                .FirstOrDefault(x => x.GetType().GenericTypeArguments[0].Name == _currentlyShowingName);

            if (targetFeat == null)
                return null;

            if (_currentlyShowingName == _featureDrawer?.Name)
                return _featureDrawer;

            _featureDrawer = new FeatureStateDrawer(targetFeat, _currentlyShowingName);
            return _featureDrawer;
        }
    }
}