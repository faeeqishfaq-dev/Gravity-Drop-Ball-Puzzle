using UnityEngine;
using UnityEditor;

namespace PhysicsDraw.EditorScripts
{
    public static class FixLevel11Setup
    {
        [MenuItem("PhysicsDraw/Fix Level 11 Logic")]
        public static void Fix()
        {
            string path = "Assets/_Game/Prefabs/Levels/Level_011.prefab";
            if (!System.IO.File.Exists(path)) return;

            GameObject instance = PrefabUtility.LoadPrefabContents(path);
            if (instance == null) return;
            
            bool modified = false;

            // Add GrayBall to all grey balls
            foreach (Transform child in instance.transform)
            {
                if (child.name.Contains("GrayBall"))
                {
                    if (child.GetComponent<PhysicsDraw.Level.GrayBall>() == null)
                    {
                        child.gameObject.AddComponent<PhysicsDraw.Level.GrayBall>();
                        modified = true;
                    }
                }
                
                if (child.name == "Goal")
                {
                    if (child.GetComponent<PhysicsDraw.Level.GrayBallTracker>() == null)
                    {
                        child.gameObject.AddComponent<PhysicsDraw.Level.GrayBallTracker>();
                        modified = true;
                    }
                }
            }

            if (modified)
            {
                PrefabUtility.SaveAsPrefabAsset(instance, path);
                Debug.Log("Level 11 modified successfully with GrayBall logic.");
            }

            PrefabUtility.UnloadPrefabContents(instance);
        }
    }
}
