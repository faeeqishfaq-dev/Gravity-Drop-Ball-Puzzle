using UnityEditor;
using UnityEngine;

namespace PhysicsDraw.EditorScripts
{
    public static class Phase14_5_PolishPrefabs
    {
        [MenuItem("PhysicsDraw/Phase 14.5 - Polish Prefabs")]
        public static void PolishPrefabs()
        {
            // 1. Polish Ball
            GameObject ballPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Prefabs/Ball.prefab");
            if (ballPrefab != null)
            {
                var inst = (GameObject)PrefabUtility.InstantiatePrefab(ballPrefab);
                var sr = inst.GetComponent<SpriteRenderer>();
                sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/Circle.png");
                sr.color = new Color(0.9f, 0.1f, 0.1f); // Red
                sr.transform.localScale = new Vector3(0.8f, 0.8f, 1f); // Make it slightly smaller/tighter
                
                var col = inst.GetComponent<CircleCollider2D>();
                col.radius = 0.5f;

                PrefabUtility.SaveAsPrefabAsset(inst, "Assets/_Game/Prefabs/Ball.prefab");
                GameObject.DestroyImmediate(inst);
            }

            // 2. Polish Goal (Make it a U-shape)
            GameObject goalPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Prefabs/Goal.prefab");
            if (goalPrefab != null)
            {
                var inst = (GameObject)PrefabUtility.InstantiatePrefab(goalPrefab);
                
                // Clear old visual (assuming it's a SpriteRenderer on the root or children)
                var oldSr = inst.GetComponent<SpriteRenderer>();
                if (oldSr != null) GameObject.DestroyImmediate(oldSr, true);
                
                // Build U shape
                CreateUWall(inst.transform, "LeftWall", new Vector3(-0.6f, 0, 0), new Vector2(0.2f, 1.2f));
                CreateUWall(inst.transform, "RightWall", new Vector3(0.6f, 0, 0), new Vector2(0.2f, 1.2f));
                CreateUWall(inst.transform, "BottomWall", new Vector3(0, -0.5f, 0), new Vector2(1.4f, 0.2f));

                var bc = inst.GetComponent<BoxCollider2D>();
                if (bc != null)
                {
                    bc.size = new Vector2(1f, 0.8f);
                    bc.offset = new Vector2(0, 0.1f);
                    bc.isTrigger = true;
                }

                PrefabUtility.SaveAsPrefabAsset(inst, "Assets/_Game/Prefabs/Goal.prefab");
                GameObject.DestroyImmediate(inst);
            }

            // 3. Fix Build Settings
            var scenes = new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene("Assets/_Game/Scenes/Bootstrap.unity", true),
                new EditorBuildSettingsScene("Assets/_Game/Scenes/MainMenu.unity", true),
                new EditorBuildSettingsScene("Assets/_Game/Scenes/Gameplay.unity", true)
            };
            EditorBuildSettings.scenes = scenes;

            Debug.Log("[Phase 14.5] Prefabs polished and Build Settings updated.");
        }

        private static void CreateUWall(Transform parent, string name, Vector3 pos, Vector2 size)
        {
            var existing = parent.Find(name);
            GameObject wall;
            if (existing != null)
            {
                wall = existing.gameObject;
            }
            else
            {
                wall = new GameObject(name);
                wall.transform.SetParent(parent, false);
            }
            
            wall.transform.localPosition = pos;
            var sr = wall.GetComponent<SpriteRenderer>();
            if (sr == null) sr = wall.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/RoundedRect.png");
            sr.drawMode = SpriteDrawMode.Sliced;
            sr.size = size;
            sr.color = new Color(0.1f, 0.8f, 0.8f); // Cyan/Green as requested

            var bc = wall.GetComponent<BoxCollider2D>();
            if (bc == null) bc = wall.AddComponent<BoxCollider2D>();
            bc.size = size;
            bc.isTrigger = false;
        }
    }
}
