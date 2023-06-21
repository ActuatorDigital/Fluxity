using UnityEditor;
using UnityEditor.SettingsManagement;

namespace AIR.Fluxity.Editor
{
    internal class FluxityEditorSettings
    {
        internal const string SETTINGS_KEY = "com.air.fluxity.editor.settings";

        static Settings _instance;

        internal static Settings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Settings(SETTINGS_KEY);
                
                return _instance;
            }
        }

        internal static class FluxityEditorSettingsProvider
        {
            const string k_PreferencesPath = "Preferences/Fluxity";

            [SettingsProvider]
            static SettingsProvider CreateSettingsProvider()
            {
                var provider = new UserSettingsProvider(k_PreferencesPath,
                    Instance,
                    new[] { typeof(FluxityEditorSettingsProvider).Assembly });

                return provider;
            }
        }
    }
}