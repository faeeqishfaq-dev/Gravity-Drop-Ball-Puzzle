using UnityEngine;
using UnityEditor;
using System.IO;
using PhysicsDraw.Level;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase17_CreateLevels11_12
    {
        static Phase17_CreateLevels11_12()
        {
            if (!SessionState.GetBool("Phase17_Done", false))
            {
                SessionState.SetBool("Phase17_Done", true);
                EditorApplication.delayCall += CreateLevels;
            }
        }

        [MenuItem("PhysicsDraw/Phase 17 - Create Levels 11 and 12")]
        public static void CreateLevels()
        {
            if (EditorApplication.isPlaying) return;

            // 1. Generate Star Image
            GenerateStarImage();

            // 2. Create Star Prefab
            CreateStarPrefab();

            // 3. Create Level 11 and 12 Prefabs
            BuildLevel(11, "The Avalanche", BuildLevel11);
            BuildLevel(12, "Starry Night", BuildLevel12);

            // 4. Register to Database
            RegisterToDatabase();

            Debug.Log("[Phase 17] Levels 11 and 12 created and registered perfectly!");
        }

        private static void GenerateStarImage()
        {
            string path = "Assets/_Game/Art/UI/Star.png";
            int w = 256; int h = 256;
            Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);

            float starOuter = w / 2f * 0.9f;
            float starInner = starOuter * 0.382f;
            Vector2 center = new Vector2(w / 2f, h / 2f);

            Vector2[] starPts = new Vector2[10];
            for (int i = 0; i < 10; i++)
            {
                float r = (i % 2 == 0) ? starOuter : starInner;
                float ang = Mathf.PI / 2f + i * (Mathf.PI * 2f / 10f);
                starPts[i] = new Vector2(Mathf.Cos(ang) * r, Mathf.Sin(ang) * r) + center;
            }

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    bool inside = false;
                    for (int i = 0, j = 9; i < 10; j = i++)
                    {
                        if (((starPts[i].y > y) != (starPts[j].y > y)) &&
                            (x < (starPts[j].x - starPts[i].x) * (y - starPts[i].y) / (starPts[j].y - starPts[i].y) + starPts[i].x))
                        {
                            inside = !inside;
                        }
                    }
                    tex.SetPixel(x, y, inside ? Color.black : Color.clear);
                }
            }
            tex.Apply();
            File.WriteAllBytes(path, tex.EncodeToPNG());
            Object.DestroyImmediate(tex);

            AssetDatabase.Refresh();

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null && importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.SaveAndReimport();
            }
        }

        private static void CreateStarPrefab()
        {
            string path = "Assets/_Game/Prefabs/Mechanics/StarObstacle.prefab";
            if (File.Exists(path)) return;

            GameObject starObj = new GameObject("StarObstacle");
            var sr = starObj.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Game/Art/UI/Star.png");
            sr.color = Color.black;

            var poly = starObj.AddComponent<PolygonCollider2D>();
            // Auto-generates from sprite

            PrefabUtility.SaveAsPrefabAsset(starObj, path);
            GameObject.DestroyImmediate(starObj);
        }

        private static void BuildLevel11(GameObject root)
        {
            // Ball on the right vertical block
            Spawn(root, "Ball", new Vector3(3.5f, 2f, 0));

            // Goal lower left
            Spawn(root, "Goal", new Vector3(-1.5f, -3f, 0));

            // Left sloped platform (descending left to right)
            var leftSlope = SpawnPlatform(root, new Vector3(-1.5f, 2f, 0), new Vector2(6f, 0.5f));
            leftSlope.transform.eulerAngles = new Vector3(0, 0, -20);

            // Right sloped platform (ascending left to right)
            var rightSlope = SpawnPlatform(root, new Vector3(1.5f, -1f, 0), new Vector2(5f, 0.5f));
            rightSlope.transform.eulerAngles = new Vector3(0, 0, 30);

            // Tall vertical block
            var vertBlock = SpawnPlatform(root, new Vector3(3.5f, -1f, 0), new Vector2(1.5f, 5f));

            // Gray balls on the left slope
            SpawnGrayBall(root, new Vector3(-3f, 3.5f, 0));
            SpawnGrayBall(root, new Vector3(-2.5f, 3.2f, 0));
            SpawnGrayBall(root, new Vector3(-2f, 3f, 0));
            SpawnGrayBall(root, new Vector3(-1.5f, 2.7f, 0));
            SpawnGrayBall(root, new Vector3(-1f, 2.5f, 0));
            SpawnGrayBall(root, new Vector3(-0.5f, 2.2f, 0));
            
            // Lower gray ball
            SpawnGrayBall(root, new Vector3(0.5f, 1f, 0));

            // Blue border frame
            BuildBlueBorder(root, new Vector2(0, 0.5f), 9, 10);
        }

        private static void BuildLevel12(GameObject root)
        {
            // Goal on the left
            Spawn(root, "Goal", new Vector3(-3f, 0, 0));

            // Small platform on right
            SpawnPlatform(root, new Vector3(3.5f, 0.5f, 0), new Vector2(1.5f, 0.5f));

            // Ball on right platform
            Spawn(root, "Ball", new Vector3(3.5f, 1.2f, 0));

            // Three stars in the middle
            SpawnStar(root, new Vector3(-1f, 0.5f, 0));
            SpawnStar(root, new Vector3(0.8f, 0.5f, 0));
            SpawnStar(root, new Vector3(2.6f, 0.5f, 0));

            // Blue border frame
            BuildBlueBorder(root, new Vector2(0, 0.5f), 9, 10);
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

        private static void SpawnGrayBall(GameObject root, Vector3 pos)
        {
            var ball = Spawn(root, "Ball", pos);
            ball.name = "GrayBall";
            ball.tag = "Untagged"; // So it doesn't trigger the goal
            var sr = ball.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = new Color(0.4f, 0.4f, 0.4f);
        }

        private static GameObject SpawnStar(GameObject root, Vector3 pos)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Prefabs/Mechanics/StarObstacle.prefab");
            if (prefab == null) return null;
            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, root.transform);
            obj.transform.localPosition = pos;
            // scale it to be fairly large
            obj.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            return obj;
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
            // The blue border visually defines the bounds, let's use it or expand by 0.5f.
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
                var d11 = AssetDatabase.LoadAssetAtPath<LevelData>("Assets/_Game/Resources/Levels/LevelData_011.asset");
                var d12 = AssetDatabase.LoadAssetAtPath<LevelData>("Assets/_Game/Resources/Levels/LevelData_012.asset");
                
                if (d11 != null && !db.allLevels.Contains(d11)) db.allLevels.Add(d11);
                if (d12 != null && !db.allLevels.Contains(d12)) db.allLevels.Add(d12);
                
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
