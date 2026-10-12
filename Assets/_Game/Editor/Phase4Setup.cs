using UnityEditor;
using UnityEngine;
using PhysicsDraw.Level;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase4Setup
    {
        static Phase4Setup()
        {
            if (!SessionState.GetBool("Phase4SetupDone", false))
            {
                EditorApplication.delayCall += RunSetup;
                SessionState.SetBool("Phase4SetupDone", true);
            }
        }

        [MenuItem("PhysicsDraw/Run Phase 4 Setup")]
        public static void RunSetup()
        {
            CreateLevelDatabase();
            Debug.Log("[Phase4Setup] Phase 4 Setup Complete.");
        }

        private static void CreateLevelDatabase()
        {
            string resPath = "Assets/_Game/Resources";
            if (!AssetDatabase.IsValidFolder(resPath))
            {
                AssetDatabase.CreateFolder("Assets/_Game", "Resources");
            }

            LevelDatabase db = AssetDatabase.LoadAssetAtPath<LevelDatabase>(resPath + "/LevelDatabase.asset");
            if (db == null)
            {
                db = ScriptableObject.CreateInstance<LevelDatabase>();
                AssetDatabase.CreateAsset(db, resPath + "/LevelDatabase.asset");
            }

            AssetDatabase.SaveAssets();

            // Setup LevelManager in TestScene
            string scenePath = "Assets/_Game/Scenes/TestPhysics.unity";
            if (System.IO.File.Exists(scenePath))
            {
                var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);
                var gmObj = GameObject.Find("GameManager");
                if (gmObj != null)
                {
                    if (gmObj.GetComponent<LevelManager>() == null)
                        gmObj.AddComponent<LevelManager>();
                }
                
                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene, scenePath);
            }
        }
    }
}
