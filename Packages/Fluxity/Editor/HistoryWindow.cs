using UnityEngine;
using UnityEditor;

namespace AIR.Fluxity.Editor
{
    class HistoryWindow : FluxityRuntimeEditorWindow
    {
        [MenuItem("Window/Fluxity/Runtime History")]
        public static void ShowWindow()
        {
            // Get existing open window or if none, make a new one:
            var window = GetWindow<HistoryWindow>();
            window.titleContent = new GUIContent("Fluxity Runtime History");
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
            }
        }
    }
}