using UnityEditor;
using UnityEngine;
using PhysicsDraw.Level;

namespace PhysicsDraw.EditorScripts
{
    public static class Phase14_4_RebuildLevels
    {
        [MenuItem("PhysicsDraw/Phase 14.4 - Rebuild Levels 1-10 Visually")]
        public static void RebuildLevels()
        {
            // Level 1: Tutorial
            GenerateLevel(1, "The Basics", l => 
            {
                Spawn(l, "Ball", new Vector3(-4, 3, 0));
                Spawn(l, "Goal", new Vector3(4, -4, 0));
                SpawnPlatform(l, new Vector3(4, -5, 0), new Vector2(4, 1f));
                
                // Tutorial text
                GameObject textObj = new GameObject("TutorialText");
                textObj.transform.SetParent(l.transform);
                textObj.transform.position = new Vector3(0, 5, 0);
                var mesh = textObj.AddComponent<TextMesh>();
                mesh.text = "Draw a line to guide the ball!";
                mesh.color = new Color(0.2f, 0.2f, 0.2f);
                mesh.anchor = TextAnchor.MiddleCenter;
                mesh.characterSize = 0.5f;
                mesh.fontSize = 60;
            });

            // Level 2: The Gap
            GenerateLevel(2, "The Gap", l => 
            {
                Spawn(l, "Ball", new Vector3(-6, 2, 0));
                Spawn(l, "Goal", new Vector3(6, -2, 0));
                SpawnPlatform(l, new Vector3(-6, 0, 0), new Vector2(3, 1f));
                SpawnPlatform(l, new Vector3(6, -4, 0), new Vector2(3, 1f));
                Spawn(l, "Mechanics/Spikes", new Vector3(0, -7, 0), new Vector2(16, 1f));
            });

            // Level 3: Falling weight / Push
            GenerateLevel(3, "Push It", l => 
            {
                Spawn(l, "Ball", new Vector3(-3, -3, 0));
                Spawn(l, "Goal", new Vector3(6, -3, 0));
                SpawnPlatform(l, new Vector3(1.5f, -4, 0), new Vector2(12, 1f)); 
                // The player must drop a heavy drawing from above to hit the ball and push it right.
                GameObject textObj = new GameObject("Hint");
                textObj.transform.SetParent(l.transform);
                textObj.transform.position = new Vector3(0, 3, 0);
                var mesh = textObj.AddComponent<TextMesh>();
                mesh.text = "Drop something heavy!";
                mesh.color = new Color(0.5f, 0.5f, 0.5f);
                mesh.anchor = TextAnchor.MiddleCenter;
                mesh.characterSize = 0.4f;
                mesh.fontSize = 60;
            });

            // Level 4: Catch the Ball
            GenerateLevel(4, "Catch & Slide", l => 
            {
                Spawn(l, "Ball", new Vector3(0, 6, 0)); // Ball falling down fast
                Spawn(l, "Goal", new Vector3(5, -5, 0));
                SpawnPlatform(l, new Vector3(5, -6, 0), new Vector2(3, 1f));
                Spawn(l, "Mechanics/Spikes", new Vector3(0, -8, 0), new Vector2(16, 1f));
            });

            // Level 5: Funnel
            GenerateLevel(5, "Funnel", l => 
            {
                Spawn(l, "Ball", new Vector3(0, 5, 0));
                Spawn(l, "Goal", new Vector3(0, -6, 0));
                SpawnPlatform(l, new Vector3(-4, 0, 0), new Vector2(4, 1f));
                SpawnPlatform(l, new Vector3(4, 0, 0), new Vector2(4, 1f));
                // Player needs to draw a funnel to catch it and drop it gently.
            });

            // Level 6: Redirect
            GenerateLevel(6, "Redirect", l => 
            {
                Spawn(l, "Ball", new Vector3(-5, 4, 0));
                Spawn(l, "Goal", new Vector3(5, 4, 0));
                SpawnPlatform(l, new Vector3(0, -2, 0), new Vector2(4, 1f));
                // Player must draw a U-shape to catch and ramp it up to the other side.
            });

            // Level 7: Moving Platform
            GenerateLevel(7, "The Elevator", l => 
            {
                Spawn(l, "Ball", new Vector3(-5, -2, 0));
                Spawn(l, "Goal", new Vector3(5, 5, 0));
                SpawnPlatform(l, new Vector3(-5, -3, 0), new Vector2(3, 1f));
                SpawnPlatform(l, new Vector3(5, 4, 0), new Vector2(3, 1f));
                
                var plat = Spawn(l, "Mechanics/MovingPlatform", new Vector3(0, -3, 0), new Vector2(3, 0.5f));
                var mp = plat.GetComponent<PhysicsDraw.Mechanics.MovingPlatform>();
                mp.pointA = new Vector3(0, -3, 0);
                mp.pointB = new Vector3(0, 4, 0);
                mp.speed = 2f;
            });

            // Level 8: Bounce
            GenerateLevel(8, "Bounce", l => 
            {
                Spawn(l, "Ball", new Vector3(-6, 5, 0));
                Spawn(l, "Goal", new Vector3(6, 5, 0));
                Spawn(l, "Mechanics/Spring", new Vector3(0, -4, 0), new Vector2(2, 1f));
                SpawnPlatform(l, new Vector3(-6, 4, 0), new Vector2(2, 1f));
                SpawnPlatform(l, new Vector3(6, 4, 0), new Vector2(2, 1f));
            });

            // Level 9: Hook
            GenerateLevel(9, "The Hook", l => 
            {
                Spawn(l, "Ball", new Vector3(-4, -4, 0));
                Spawn(l, "Goal", new Vector3(4, 4, 0));
                SpawnPlatform(l, new Vector3(-4, -5, 0), new Vector2(3, 1f));
                SpawnPlatform(l, new Vector3(4, 3, 0), new Vector2(3, 1f));
                // A static floating block to hook a drawing onto
                SpawnPlatform(l, new Vector3(0, 6, 0), new Vector2(2, 2f));
            });

            // Level 10: Final Challenge
            GenerateLevel(10, "Final Challenge", l => 
            {
                Spawn(l, "Ball", new Vector3(-6, 6, 0));
                Spawn(l, "Goal", new Vector3(6, -6, 0));
                SpawnPlatform(l, new Vector3(-6, 5, 0), new Vector2(2, 1f));
                SpawnPlatform(l, new Vector3(6, -7, 0), new Vector2(4, 1f));
                
                var plat = Spawn(l, "Mechanics/MovingPlatform", new Vector3(-3, 0, 0), new Vector2(2, 0.5f));
                var mp = plat.GetComponent<PhysicsDraw.Mechanics.MovingPlatform>();
                mp.pointA = new Vector3(-3, 0, 0);
                mp.pointB = new Vector3(3, 0, 0);
                mp.speed = 3f;

                Spawn(l, "Mechanics/Spikes", new Vector3(0, -9, 0), new Vector2(20, 1f));
            });

            Debug.Log("[Phase 14.4] Levels 1-10 visually polished and built.");
        }

        private static GameObject SpawnPlatform(GameObject parent, Vector3 pos, Vector2 size)
        {
            var obj = Spawn(parent, "StaticPlatform", pos, size);
            var sr = obj.GetComponent<SpriteRenderer>();
            sr.color = new Color(0.2f, 0.2f, 0.2f); // Dark charcoal as requested
            return obj;
        }

        private static void GenerateLevel(int id, string name, System.Action<GameObject> populateFunc)
        {
            string prefabPath = $"Assets/_Game/Prefabs/Levels/Level_{id:D3}.prefab";
            string dataPath = $"Assets/_Game/Resources/Levels/LevelData_{id:D3}.asset";

            GameObject levelRoot = new GameObject($"Level_{id:D3}");

            populateFunc?.Invoke(levelRoot);

            PrefabUtility.SaveAsPrefabAsset(levelRoot, prefabPath);
            GameObject.DestroyImmediate(levelRoot);

            LevelData data = AssetDatabase.LoadAssetAtPath<LevelData>(dataPath);
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<LevelData>();
                AssetDatabase.CreateAsset(data, dataPath);
            }
            data.levelID = id;
            data.displayName = name;
            data.levelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            EditorUtility.SetDirty(data);
            
            AssetDatabase.SaveAssets();
        }

        private static GameObject Spawn(GameObject parent, string prefabName, Vector3 pos, Vector2? size = null)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/_Game/Prefabs/{prefabName}.prefab");
            if (prefab == null) return null;

            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            obj.transform.SetParent(parent.transform);
            obj.transform.position = pos;

            if (size.HasValue)
            {
                var sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null && sr.drawMode == SpriteDrawMode.Sliced) sr.size = size.Value;
                
                var bc = obj.GetComponent<BoxCollider2D>();
                if (bc != null) bc.size = size.Value;
            }
            return obj;
        }
    }
}
