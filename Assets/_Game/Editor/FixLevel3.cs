using UnityEngine;
using UnityEditor;

namespace PhysicsDraw.EditorScripts
{
    public static class FixLevel3
    {
        [MenuItem("PhysicsDraw/Fix Level 3 Platform")]
        public static void Fix()
        {
            string path = "Assets/_Game/Prefabs/Levels/Level_003.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError("Level_003 prefab not found.");
                return;
            }

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            
            bool modified = false;
            foreach (Transform child in instance.transform)
            {
                if (child.name.Contains("StaticPlatform"))
                {
                    var sr = child.GetComponent<SpriteRenderer>();
                    var bc = child.GetComponent<BoxCollider2D>();
                    
                    if (sr != null && bc != null)
                    {
                        // The user said: "collider matches visible platform"
                        // So whatever the SpriteRenderer's size is, the BoxCollider2D should match exactly.
                        if (bc.size != sr.size || bc.isTrigger)
                        {
                            bc.size = sr.size;
                            bc.isTrigger = false;
                            
                            // Ensure it's active and enabled
                            bc.enabled = true;
                            child.gameObject.SetActive(true);

                            // Force save the override
                            PrefabUtility.RecordPrefabInstancePropertyModifications(bc);
                            modified = true;
                            Debug.Log($"[FixLevel3] Fixed Platform {child.name} -> Size: {bc.size}, isTrigger: {bc.isTrigger}");
                        }
                    }
                }
            }

            if (modified)
            {
                PrefabUtility.SaveAsPrefabAsset(instance, path);
                Debug.Log("[FixLevel3] Level 3 Prefab successfully saved!");
            }
            else
            {
                Debug.Log("[FixLevel3] No changes needed or no StaticPlatform found.");
            }

            GameObject.DestroyImmediate(instance);
        }
    }
}
