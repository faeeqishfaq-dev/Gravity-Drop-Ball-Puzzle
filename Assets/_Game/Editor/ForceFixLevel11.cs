using UnityEngine;
using UnityEditor;

namespace PhysicsDraw.EditorScripts
{
    public static class ForceFixLevel11
    {
        public static void ForceRun()
        {
            string path = "Assets/_Game/Prefabs/Levels/Level_011.prefab";
            GameObject instance = PrefabUtility.LoadPrefabContents(path);
            
            int grayBallCount = 0;
            int goalCount = 0;

            foreach (Transform child in instance.transform)
            {
                if (child.name.Contains("GrayBall") || child.name.Contains("Gray Ball") || child.name.Contains("Gray"))
                {
                    if (child.GetComponent<PhysicsDraw.Level.GrayBall>() == null)
                    {
                        child.gameObject.AddComponent<PhysicsDraw.Level.GrayBall>();
                    }
                    grayBallCount++;
                }
                
                if (child.name.Contains("Goal"))
                {
                    if (child.GetComponent<PhysicsDraw.Level.GrayBallTracker>() == null)
                    {
                        child.gameObject.AddComponent<PhysicsDraw.Level.GrayBallTracker>();
                    }
                    goalCount++;
                }
                
                if (child.name.Contains("StaticPlatform") || child.name.Contains("Platform"))
                {
                    var sr = child.GetComponent<SpriteRenderer>();
                    var bc = child.GetComponent<BoxCollider2D>();
                    if (sr != null && bc != null)
                    {
                        bc.size = sr.size;
                        bc.isTrigger = false;
                        bc.enabled = true;
                    }
                }
            }

            PrefabUtility.SaveAsPrefabAsset(instance, path);
            PrefabUtility.UnloadPrefabContents(instance);
            
            Debug.Log($"FORCE FIXED LEVEL 11: GrayBalls={grayBallCount}, Goals={goalCount}");
        }
    }
}
