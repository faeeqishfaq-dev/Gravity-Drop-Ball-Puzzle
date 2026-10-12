using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase15_FixProgression
    {
        static Phase15_FixProgression()
        {
            if (!SessionState.GetBool("Phase15_Done_v2", false))
            {
                SessionState.SetBool("Phase15_Done_v2", true);
                EditorApplication.delayCall += FixAll;
            }
        }

        [MenuItem("PhysicsDraw/Fix Progression & Gameplay Screen")]
        public static void FixAll()
        {
            if (EditorApplication.isPlaying) return;

            // 1. Fix Gameplay Scene (Add Camera, Remove MainMenu UI if any)
            FixGameplayScene();
            
            // 2. Fix MainMenu Scene (Add Camera)
            FixMainMenuScene();

            Debug.Log("[Phase 15] Progression and Visibility Fixed!");
        }

        private static void FixMainMenuScene()
        {
            var scene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/MainMenu.unity", OpenSceneMode.Single);
            
            bool hasCamera = false;
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.GetComponent<Camera>() != null) hasCamera = true;
            }

            if (!hasCamera)
            {
                GameObject camObj = new GameObject("Main Camera");
                camObj.tag = "MainCamera";
                var cam = camObj.AddComponent<Camera>();
                cam.orthographic = true;
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = new Color(0.95f, 0.95f, 0.95f);
                camObj.AddComponent<AudioListener>();

                foreach (var go in scene.GetRootGameObjects())
                {
                    if (go.name == "MainMenuCanvas")
                    {
                        var canvas = go.GetComponent<Canvas>();
                        canvas.renderMode = RenderMode.ScreenSpaceCamera;
                        canvas.worldCamera = cam;
                        canvas.planeDistance = 5;
                    }
                }
            }
            EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/MainMenu.unity");
        }

        private static void FixGameplayScene()
        {
            var scene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/Gameplay.unity", OpenSceneMode.Single);
            
            bool hasCamera = false;
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.GetComponent<Camera>() != null) hasCamera = true;
                
                // Clean accidental MainMenu UI inside GameplayCanvas
                if (go.name == "GameplayCanvas")
                {
                    var header = go.transform.Find("HeaderTitle");
                    if (header != null) GameObject.DestroyImmediate(header.gameObject);
                    var play = go.transform.Find("PlayButton");
                    if (play != null) GameObject.DestroyImmediate(play.gameObject);
                    var lvl = go.transform.Find("LevelsButton");
                    if (lvl != null) GameObject.DestroyImmediate(lvl.gameObject);
                    
                    // Attach the camera to the canvas so it renders behind UI
                    var canvas = go.GetComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    // We will set the camera below
                }
            }

            if (!hasCamera)
            {
                GameObject camObj = new GameObject("Main Camera");
                camObj.tag = "MainCamera";
                var cam = camObj.AddComponent<Camera>();
                cam.orthographic = true;
                cam.orthographicSize = 10;
                cam.clearFlags = CameraClearFlags.SolidColor;
                cam.backgroundColor = new Color(0.95f, 0.95f, 0.95f); // Match notebook
                camObj.AddComponent<AudioListener>();

                // Set canvas camera
                foreach (var go in scene.GetRootGameObjects())
                {
                    if (go.name == "GameplayCanvas")
                    {
                        var canvas = go.GetComponent<Canvas>();
                        canvas.worldCamera = cam;
                        canvas.planeDistance = 5;
                    }
                }
            }

            // Ensure LevelRoot exists
            GameObject levelRoot = GameObject.Find("LevelRoot");
            if (levelRoot == null)
            {
                levelRoot = new GameObject("LevelRoot");
            }

            EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/Gameplay.unity");
        }
    }
}
