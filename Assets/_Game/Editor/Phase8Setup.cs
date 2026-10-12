using UnityEditor;
using UnityEngine;
using PhysicsDraw.Level;

namespace PhysicsDraw.EditorScripts
{
    public static class Phase8Setup
    {
        [MenuItem("PhysicsDraw/Generate Basic Levels (1-5)")]
        public static void GenerateInitialLevels()
        {
            GenerateLevel(1, "The Basics", l => 
            {
                Spawn(l, "Ball", new Vector3(-3, 3, 0));
                Spawn(l, "Goal", new Vector3(3, -3, 0));
                Spawn(l, "StaticPlatform", new Vector3(-3, 2, 0), new Vector2(3, 0.5f));
                Spawn(l, "StaticPlatform", new Vector3(3, -4, 0), new Vector2(3, 0.5f));
            });

            GenerateLevel(2, "The Gap", l => 
            {
                Spawn(l, "Ball", new Vector3(-4, 0, 0));
                Spawn(l, "Goal", new Vector3(4, 0, 0));
                Spawn(l, "StaticPlatform", new Vector3(-4, -1, 0), new Vector2(4, 1f));
                Spawn(l, "StaticPlatform", new Vector3(4, -1, 0), new Vector2(4, 1f));
                Spawn(l, "Mechanics/Spikes", new Vector3(0, -6, 0), new Vector2(12, 1f));
            });

            GenerateLevel(3, "Push It", l => 
            {
                Spawn(l, "Ball", new Vector3(0, 0, 0));
                Spawn(l, "Goal", new Vector3(6, -2, 0));
                Spawn(l, "StaticPlatform", new Vector3(0, -1, 0), new Vector2(10, 1f)); // Long flat floor
            });

            GenerateLevel(4, "Catch and Slide", l => 
            {
                Spawn(l, "Ball", new Vector3(-4, 4, 0));
                Spawn(l, "Goal", new Vector3(4, -4, 0));
                Spawn(l, "StaticPlatform", new Vector3(4, -5, 0), new Vector2(4, 1f));
            });

            GenerateLevel(5, "Moving Up", l => 
            {
                Spawn(l, "Ball", new Vector3(-4, 0, 0));
                Spawn(l, "Goal", new Vector3(4, 0, 0));
                Spawn(l, "StaticPlatform", new Vector3(-4, -1, 0), new Vector2(3, 1f));
                Spawn(l, "StaticPlatform", new Vector3(4, -1, 0), new Vector2(3, 1f));
                
                var plat = Spawn(l, "Mechanics/MovingPlatform", new Vector3(0, -1, 0), new Vector2(3, 0.5f));
                var mp = plat.GetComponent<PhysicsDraw.Mechanics.MovingPlatform>();
                mp.pointA = new Vector3(0, 0, 0);
                mp.pointB = new Vector3(0, 3, 0);
                mp.speed = 2f;
            });

            Debug.Log("[Phase8Setup] Generated Levels 1-5.");
        }

        private static void GenerateLevel(int id, string name, System.Action<GameObject> populateFunc)
        {
            string prefabPath = $"Assets/_Game/Prefabs/Levels/Level_{id:D3}.prefab";
            string dataPath = $"Assets/_Game/Resources/Levels/LevelData_{id:D3}.asset";

            if (!AssetDatabase.IsValidFolder("Assets/_Game/Prefabs/Levels"))
                AssetDatabase.CreateFolder("Assets/_Game/Prefabs", "Levels");
            if (!AssetDatabase.IsValidFolder("Assets/_Game/Resources/Levels"))
                AssetDatabase.CreateFolder("Assets/_Game/Resources", "Levels");

            // 1. Create Root
            GameObject levelRoot = new GameObject($"Level_{id:D3}");

            // 2. Populate
            populateFunc?.Invoke(levelRoot);

            // 3. Save Prefab
            PrefabUtility.SaveAsPrefabAsset(levelRoot, prefabPath);
            GameObject.DestroyImmediate(levelRoot);

            // 4. Update Level Data
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

            // 5. Register
            LevelDatabase db = Resources.Load<LevelDatabase>("LevelDatabase");
            if (db != null && !db.allLevels.Contains(data))
            {
                db.allLevels.Add(data);
                EditorUtility.SetDirty(db);
            }

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
