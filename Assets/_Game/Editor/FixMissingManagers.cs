using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class FixMissingManagers
    {
        static FixMissingManagers()
        {
            if (!SessionState.GetBool("FixMissingManagers_Done_v2", false))
            {
                SessionState.SetBool("FixMissingManagers_Done_v2", true);
                EditorApplication.delayCall += Fix;
            }
        }
        [MenuItem("PhysicsDraw/Fix Missing Managers")]
        public static void Fix()
        {
            if (EditorApplication.isPlaying) return;

            var scene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/Gameplay.unity", OpenSceneMode.Single);
            
            GameObject managerObj = null;
            foreach (var go in scene.GetRootGameObjects())
            {
                if (go.GetComponent<PhysicsDraw.Scripts.Core.GameplayBootstrapper>() != null)
                {
                    managerObj = go;
                    managerObj.name = "GameplayManagers";
                    break;
                }
            }

            if (managerObj == null)
            {
                managerObj = new GameObject("GameplayManagers");
                managerObj.AddComponent<PhysicsDraw.Scripts.Core.GameplayBootstrapper>();
            }

            if (managerObj.GetComponent<PhysicsDraw.InputSystem.InputProvider>() == null)
                managerObj.AddComponent<PhysicsDraw.InputSystem.InputProvider>();

            if (managerObj.GetComponent<PhysicsDraw.Drawing.DrawingController>() == null)
                managerObj.AddComponent<PhysicsDraw.Drawing.DrawingController>();

            if (managerObj.GetComponent<PhysicsDraw.Level.LevelManager>() == null)
                managerObj.AddComponent<PhysicsDraw.Level.LevelManager>();

            EditorSceneManager.SaveScene(scene, "Assets/_Game/Scenes/Gameplay.unity");
            Debug.Log("Fixed missing managers in Gameplay scene.");
        }
    }
}
