using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace PhysicsDraw.EditorScripts
{
    public static class Phase9Setup
    {
        [MenuItem("PhysicsDraw/Run Phase 9 Setup")]
        public static void RunSetup()
        {
            SetupBootstrap();
            SetupGameplay();
            Debug.Log("[Phase9Setup] Phase 9 Setup Complete.");
        }

        private static void SetupBootstrap()
        {
            var scene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/Bootstrap.unity", OpenSceneMode.Single);
            var cm = GameObject.Find("CoreManagers");
            if (cm != null)
            {
                if (cm.GetComponent<PhysicsDraw.Audio.AudioManager>() == null)
                    cm.AddComponent<PhysicsDraw.Audio.AudioManager>();
            }
            EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/Bootstrap.unity");
        }

        private static void SetupGameplay()
        {
            var scene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/Gameplay.unity", OpenSceneMode.Single);
            
            if (GameObject.Find("GameplayBootstrapper") == null)
            {
                var gb = new GameObject("GameplayBootstrapper");
                gb.AddComponent<PhysicsDraw.Scripts.Core.GameplayBootstrapper>();
            }
            
            EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/Gameplay.unity");
        }
    }
}
