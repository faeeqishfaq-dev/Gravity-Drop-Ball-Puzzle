using UnityEditor;
using UnityEngine;
using PhysicsDraw.Level;

namespace PhysicsDraw.EditorScripts
{
    public static class Phase8bSetup
    {
        [MenuItem("PhysicsDraw/Generate Basic Levels (6-10)")]
        public static void GenerateLevels()
        {
            GenerateLevel(6, "Static Obstacle", l => 
            {
                Spawn(l, "Ball", new Vector3(-5, 3, 0));
                Spawn(l, "Goal", new Vector3(5, -3, 0));
                Spawn(l, "StaticPlatform", new Vector3(0, -1, 0), new Vector2(1, 10f)); // Tall wall
            });

            GenerateLevel(7, "Redirection", l => 
            {
                Spawn(l, "Ball", new Vector3(-6, 4, 0));
                Spawn(l, "Goal", new Vector3(0, 0, 0));
                Spawn(l, "StaticPlatform", new Vector3(-6, -6, 0), new Vector2(14, 1f));
                Spawn(l, "Mechanics/Spikes", new Vector3(0, -5, 0), new Vector2(4, 1f));
            });

            GenerateLevel(8, "Bounce In", l => 
            {
                Spawn(l, "Ball", new Vector3(-5, 0, 0));
                Spawn(l, "Goal", new Vector3(5, 5, 0));
                Spawn(l, "Mechanics/Spring", new Vector3(-2, -5, 0));
            });

            GenerateLevel(9, "Moving Ride", l => 
            {
                Spawn(l, "Ball", new Vector3(-6, 0, 0));
                Spawn(l, "Goal", new Vector3(6, 0, 0));
                var plat = Spawn(l, "Mechanics/MovingPlatform", new Vector3(-6, -2, 0), new Vector2(3, 0.5f));
                var mp = plat.GetComponent<PhysicsDraw.Mechanics.MovingPlatform>();
                mp.pointA = new Vector3(0, 0, 0);
                mp.pointB = new Vector3(12, 0, 0);
                mp.speed = 3f;
            });

            GenerateLevel(10, "Fundamentals", l => 
            {
                Spawn(l, "Ball", new Vector3(-6, 5, 0));
                Spawn(l, "Goal", new Vector3(6, -5, 0));
                Spawn(l, "StaticPlatform", new Vector3(-2, 0, 0), new Vector2(1, 6f));
                Spawn(l, "StaticPlatform", new Vector3(2, -2, 0), new Vector2(1, 6f));
                Spawn(l, "Mechanics/MovingPlatform", new Vector3(0, -6, 0), new Vector2(2, 0.5f));
            });

            Debug.Log("[Phase8bSetup] Generated Levels 6-10.");
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
