using UnityEngine;
using UnityEditor;
using System.IO;
using PhysicsDraw.Level;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase18_CreateLevel13
    {
        static Phase18_CreateLevel13()
        {
            if (!SessionState.GetBool("Phase18_Done", false))
            {
                SessionState.SetBool("Phase18_Done", true);
                EditorApplication.delayCall += CreateLevel13;
            }
        }

        [MenuItem("PhysicsDraw/Phase 18 - Create Level 13")]
        public static void CreateLevel13()
        {
            if (EditorApplication.isPlaying) return;

            BuildLevel(13, "The Shelf", BuildLevel13);
            RegisterToDatabase();

            Debug.Log("[Phase 18] Level 13 created and registered perfectly!");
        }

        private static void BuildLevel13(GameObject root)
        {
            // Right long vertical pillar (from bottom to top border)
            SpawnPlatform(root, new Vector3(2.5f, 0, 0), new Vector2(1.5f, 10f));

            // Horizontal shelf extending from right pillar to the left
            SpawnPlatform(root, new Vector3(-0.5f, 2f, 0), new Vector2(4.5f, 1f));

            // Lower-left vertical pillar (from bottom border up to mid-height)
            SpawnPlatform(root, new Vector3(-3f, -3f, 0), new Vector2(1f, 4f));

            // Goal on the bottom center
            Spawn(root, "Goal", new Vector3(-0.25f, -4f, 0));

            // Red ball resting on the left side of the horizontal shelf
            Spawn(root, "Ball", new Vector3(-2f, 3f, 0));

            // Blue border frame around the gameplay area
            BuildBlueBorder(root, new Vector2(0, 0), 9, 10.3f);
        }

        private static void BuildBlueBorder(GameObject root, Vector2 center, float width, float height)
        {
            GameObject borderParent = new GameObject("BlueBorder");
            borderParent.transform.SetParent(root.transform);
            borderParent.transform.localPosition = center;

            float thickness = 0.15f;
            Color blue = new Color(0, 0.4f, 1f);

            // Top
            CreateBorderLine(borderParent, new Vector2(0, height / 2f), new Vector2(width, thickness), blue);
            // Bottom
            CreateBorderLine(borderParent, new Vector2(0, -height / 2f), new Vector2(width, thickness), blue);
            // Left
            CreateBorderLine(borderParent, new Vector2(-width / 2f, 0), new Vector2(thickness, height), blue);
            // Right
            CreateBorderLine(borderParent, new Vector2(width / 2f, 0), new Vector2(thickness, height), blue);
        }

        private static void CreateBorderLine(GameObject parent, Vector2 pos, Vector2 size, Color c)
        {
            GameObject line = new GameObject("BorderLine");
            line.transform.SetParent(parent.transform);
            line.transform.localPosition = pos;
            var sr = line.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/RoundedRect.png");
            sr.drawMode = SpriteDrawMode.Sliced;
            sr.size = size;
            sr.color = c;
        }

        private static GameObject SpawnPlatform(GameObject parent, Vector3 pos, Vector2 size)
        {
            var obj = Spawn(parent, "StaticPlatform", pos, size);
            var sr = obj.GetComponent<SpriteRenderer>();
            sr.color = Color.black;
            return obj;
        }

        private static GameObject Spawn(GameObject parent, string prefabName, Vector3 pos, Vector2? size = null)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/_Game/Prefabs/{prefabName}.prefab");
            if (prefab == null) return null;
            
            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent.transform);
            obj.transform.localPosition = pos;
            if (size.HasValue)
            {
                var sr = obj.GetComponent<SpriteRenderer>();
                if (sr != null) sr.size = size.Value;
                var bc = obj.GetComponent<BoxCollider2D>();
                if (bc != null) bc.size = size.Value;
            }
            return obj;
        }

        private static void BuildLevel(int id, string title, System.Action<GameObject> populate)
        {
            string prefabPath = $"Assets/_Game/Prefabs/Levels/Level_{id:000}.prefab";
            GameObject instance = new GameObject($"Level_{id:000}");
            
            populate(instance);
            
            // Add bounds calculation
            var lb = instance.AddComponent<LevelBounds>();
            float minX = 0, maxX = 0, minY = 0, maxY = 0;
            bool first = true;
            foreach (Transform child in instance.transform)
            {
                Vector3 pos = child.localPosition;
                float w = 1f; float h = 1f;
                var sr = child.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    w = sr.bounds.size.x / 2f;
                    h = sr.bounds.size.y / 2f;
                }
                if (first)
                {
                    minX = pos.x - w; maxX = pos.x + w;
                    minY = pos.y - h; maxY = pos.y + h;
                    first = false;
                }
                else
                {
                    if (pos.x - w < minX) minX = pos.x - w;
                    if (pos.x + w > maxX) maxX = pos.x + w;
                    if (pos.y - h < minY) minY = pos.y - h;
                    if (pos.y + h > maxY) maxY = pos.y + h;
                }
            }
            // Expand by 0.5f to ensure border lines are comfortably within frame
            minX -= 0.5f; maxX += 0.5f;
            minY -= 0.5f; maxY += 0.5f;
            lb.center = new Vector2((minX + maxX) / 2f, (minY + maxY) / 2f);
            lb.size = new Vector2(maxX - minX, maxY - minY);

            PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
            GameObject.DestroyImmediate(instance);

            // Create LevelData
            string dataPath = $"Assets/_Game/Resources/Levels/LevelData_{id:000}.asset";
            LevelData data = AssetDatabase.LoadAssetAtPath<LevelData>(dataPath);
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<LevelData>();
                AssetDatabase.CreateAsset(data, dataPath);
            }
            data.levelID = id;
            data.displayName = title;
            data.levelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            data.ballStartMode = BallStartMode.OnFirstDraw;
            EditorUtility.SetDirty(data);
        }

        private static void RegisterToDatabase()
        {
            LevelDatabase db = AssetDatabase.LoadAssetAtPath<LevelDatabase>("Assets/_Game/Resources/LevelDatabase.asset");
            if (db != null)
            {
                var data = AssetDatabase.LoadAssetAtPath<LevelData>("Assets/_Game/Resources/Levels/LevelData_013.asset");
                if (data != null && !db.allLevels.Contains(data)) db.allLevels.Add(data);
                
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
