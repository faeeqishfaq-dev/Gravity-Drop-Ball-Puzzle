using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace PhysicsDraw.EditorScripts
{
    [InitializeOnLoad]
    public static class AutoBuildSettingsUI
    {
        static AutoBuildSettingsUI()
        {
            EditorApplication.update += RunOnceWhenReady;
        }

        private static void RunOnceWhenReady()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (EditorApplication.isCompiling || EditorApplication.isUpdating) return;

            EditorApplication.update -= RunOnceWhenReady;

            if (!EditorPrefs.GetBool("SettingsUIBuilt", false))
            {
                string scenePath = "Assets/_Game/Scenes/MainMenu.unity";
                Scene currentScene = EditorSceneManager.GetActiveScene();
                bool wasOtherScene = currentScene.path != scenePath;

                if (wasOtherScene)
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scenePath);
                    }
                    else return;
                }

                BuildMainMenuSettings.BuildUI();
                EditorPrefs.SetBool("SettingsUIBuilt", true);

                if (wasOtherScene)
                {
                    EditorSceneManager.OpenScene(currentScene.path);
                }
                
                Debug.Log(">>> AutoBuildSettingsUI executed automatically! <<<");
            }
        }
    }
}
