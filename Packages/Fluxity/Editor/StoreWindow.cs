using UnityEngine;
using UnityEditor;
using AIR.Flume;
using System.Linq;

namespace AIR.Fluxity.Editor
{
    class StoreWindow : EditorWindow
    {
        internal class StoreHandle : Dependent
        {
            public IStore Store { get; private set; }

            public void Inject(IStore store)
                => Store = store;
        }

        private const int LeftPanelRightWall = 220;
        private Vector2 _leftPanelScrollViewPos;
        private Vector2 _rightPanelScrollViewPos;
        private StoreHandle _storeHandle;
        private string _currentlyShowingName;
        private FeatureStateDrawer _featureDrawer;

        [MenuItem("Window/Fluxity/Runtime Stores")]
        public static void ShowWindow()
        {
            // Get existing open window or if none, make a new one:
            var window = GetWindow<StoreWindow>();
            window.titleContent = new GUIContent("Fluxity Runtime Stores");
            window.Show();
        }

        public void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        public void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        public void OnGUI()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                GUILayout.Label("Not in play mode.");
            }
            else
            {
                DoStoreSelectPanel();
                DoSelectedStore();
            }
        }

        private void DoStoreSelectPanel()
        {
            GUILayout.BeginArea(new Rect(0, 0, LeftPanelRightWall, position.height));
            {
                _leftPanelScrollViewPos = EditorGUILayout.BeginScrollView(
                    _leftPanelScrollViewPos,
                    GUILayout.Height(position.height),
                    GUILayout.Width(LeftPanelRightWall));
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
                        EditorGUILayout.LabelField("Stores", EditorStyles.boldLabel);
                        DrawHorLine(Color.grey);
                        foreach (var feat in store.GetAllFeatures())
                        {
                            var name = feat.GetType().GenericTypeArguments[0].Name;
                            if (GUILayout.Button(name, _currentlyShowingName == name ? EditorStyles.selectionRect : EditorStyles.label))
                            {
                                _currentlyShowingName = name;
                            }
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        private void DoSelectedStore()
        {
            GUILayout.BeginArea(new Rect(LeftPanelRightWall, 0, position.width - LeftPanelRightWall, position.height));
            {
                _rightPanelScrollViewPos = EditorGUILayout.BeginScrollView(
                    _rightPanelScrollViewPos,
                    GUILayout.Height(position.height));
                {
                    var drawer = GetObjectDrawerForTarget();

                    if (drawer == null)
                    {
                        EditorGUILayout.LabelField("No feature selected");
                    }
                    else
                    {
                        EditorGUILayout.LabelField($"Values of {_currentlyShowingName}", EditorStyles.boldLabel);
                        DrawHorLine(Color.grey);

                        drawer.Draw();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
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

        private void Refresh()
        {
            _storeHandle = null;
            _currentlyShowingName = null;
            _featureDrawer = null;
        }

        private IStore GetStore()
        {
            if (_storeHandle == null)
            {
                if (EditorApplication.isPlaying)
                {
                    _storeHandle = new StoreHandle();
                }
            }

            return _storeHandle?.Store;
        }

        private static void DrawHorLine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        private void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            Refresh();
            Repaint();
        }
    }
}