using LastToTheGlobe.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class MyFirstMenuItem
    {
        [MenuItem("Tool/Create planets")]
        public static void CreatePlanetGenerationWindow()
        {
            var window = ScriptableObject.CreateInstance<GP_GUI>();
            var settingsPath = "Assets/LastToTheGlobe/Scripts/ScriptableObject/GP_GUISettings.asset";

            var windowSettings = (GP_GUISettings)AssetDatabase.LoadAssetAtPath(settingsPath,
                typeof(GP_GUISettings));

            if (windowSettings == null)
            {
                windowSettings = ScriptableObject.CreateInstance<GP_GUISettings>();

                AssetDatabase.CreateAsset(windowSettings, settingsPath);
                AssetDatabase.SaveAssets();
            }

            window.GUISettings = windowSettings;

            window.Show();
        }

    }
}
