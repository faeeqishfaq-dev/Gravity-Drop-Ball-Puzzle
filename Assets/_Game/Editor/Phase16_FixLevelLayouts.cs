using UnityEngine;
using UnityEditor;
using System.IO;
using PhysicsDraw.Level;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase16_FixLevelLayouts
    {
        static Phase16_FixLevelLayouts()
        {
            if (!SessionState.GetBool("Phase16_Done_v2", false))
            {
                SessionState.SetBool("Phase16_Done_v2", true);
                EditorApplication.delayCall += RebuildForPortrait;
            }
        }

        [MenuItem("PhysicsDraw/Phase 16 - Rebuild Levels For Portrait")]
        public static void RebuildForPortrait()
        {
            if (EditorApplication.isPlaying) return;

            BuildLevel(1, l =>
            {
                Spawn(l, "Ball", new Vector3(-2.5f, 3, 0));
                Spawn(l, "Goal", new Vector3(2.5f, -4, 0));
                SpawnPlatform(l, new Vector3(2.5f, -5, 0), new Vector2(3, 1f));
            });

            BuildLevel(2, l =>
            {
                Spawn(l, "Ball", new Vector3(-3f, 2, 0));
                Spawn(l, "Goal", new Vector3(3f, -2, 0));
                SpawnPlatform(l, new Vector3(-3f, 0, 0), new Vector2(2.5f, 1f));
                SpawnPlatform(l, new Vector3(3f, -4, 0), new Vector2(2.5f, 1f));
                Spawn(l, "Mechanics/Spikes", new Vector3(0, -7, 0), new Vector2(8, 1f));
            });

            BuildLevel(3, l =>
            {
                Spawn(l, "Ball", new Vector3(-2.5f, -3, 0));
                Spawn(l, "Goal", new Vector3(2.5f, -3, 0));
                SpawnPlatform(l, new Vector3(0, -4, 0), new Vector2(7, 1f)); 
            });

            BuildLevel(4, l =>
            {
                Spawn(l, "Ball", new Vector3(0, 6, 0));
                Spawn(l, "Goal", new Vector3(2.5f, -5, 0));
                SpawnPlatform(l, new Vector3(2.5f, -6, 0), new Vector2(3, 1f));
                Spawn(l, "Mechanics/Spikes", new Vector3(0, -8, 0), new Vector2(8, 1f));
            });

            BuildLevel(5, l =>
            {
                Spawn(l, "Ball", new Vector3(0, 5, 0));
                Spawn(l, "Goal", new Vector3(0, -6, 0));
                SpawnPlatform(l, new Vector3(-2.5f, 0, 0), new Vector2(3, 1f));
                SpawnPlatform(l, new Vector3(2.5f, 0, 0), new Vector2(3, 1f));
            });

            BuildLevel(6, l =>
            {
                Spawn(l, "Ball", new Vector3(-2.5f, 4, 0));
                Spawn(l, "Goal", new Vector3(2.5f, 4, 0));
                SpawnPlatform(l, new Vector3(0, -2, 0), new Vector2(3, 1f));
            });

            BuildLevel(7, l =>
            {
                Spawn(l, "Ball", new Vector3(-2.5f, -2, 0));
                Spawn(l, "Goal", new Vector3(2.5f, 5, 0));
                SpawnPlatform(l, new Vector3(-2.5f, -3, 0), new Vector2(2, 1f));
                SpawnPlatform(l, new Vector3(2.5f, 4, 0), new Vector2(2, 1f));
                
                var plat = Spawn(l, "Mechanics/MovingPlatform", new Vector3(0, -3, 0), new Vector2(2, 0.5f));
                var mp = plat.GetComponent<PhysicsDraw.Mechanics.MovingPlatform>();
                mp.pointA = new Vector3(-1.5f, -3, 0);
                mp.pointB = new Vector3(1.5f, -3, 0);
                mp.speed = 2f;
            });

            BuildLevel(8, l =>
            {
                Spawn(l, "Ball", new Vector3(-3f, 5, 0));
                Spawn(l, "Goal", new Vector3(3f, 5, 0));
                Spawn(l, "Mechanics/Spring", new Vector3(0, -4, 0), new Vector2(2, 1f));
                SpawnPlatform(l, new Vector3(-3f, 4, 0), new Vector2(2, 1f));
                SpawnPlatform(l, new Vector3(3f, 4, 0), new Vector2(2, 1f));
            });

            BuildLevel(9, l =>
            {
                Spawn(l, "Ball", new Vector3(-2.5f, -4, 0));
                Spawn(l, "Goal", new Vector3(2.5f, 4, 0));
                SpawnPlatform(l, new Vector3(-2.5f, -5, 0), new Vector2(2.5f, 1f));
                SpawnPlatform(l, new Vector3(2.5f, 3, 0), new Vector2(2.5f, 1f));
                SpawnPlatform(l, new Vector3(0, 5, 0), new Vector2(1.5f, 1.5f));
            });

            BuildLevel(10, l =>
            {
                Spawn(l, "Ball", new Vector3(-3f, 6, 0));
                Spawn(l, "Goal", new Vector3(3f, -6, 0));
                SpawnPlatform(l, new Vector3(-3f, 5, 0), new Vector2(2, 1f));
                SpawnPlatform(l, new Vector3(3f, -7, 0), new Vector2(3, 1f));
                
                var plat = Spawn(l, "Mechanics/MovingPlatform", new Vector3(-1.5f, 0, 0), new Vector2(2, 0.5f));
                var mp = plat.GetComponent<PhysicsDraw.Mechanics.MovingPlatform>();
                mp.pointA = new Vector3(-1.5f, 0, 0);
                mp.pointB = new Vector3(1.5f, 0, 0);
                mp.speed = 3f;

                Spawn(l, "Mechanics/Spikes", new Vector3(0, -9, 0), new Vector2(8, 1f));
            });

            Debug.Log("[Phase 16] All Levels Rebuilt for Portrait Mobile!");
        }

        private static void BuildLevel(int id, System.Action<GameObject> populate)
        {
            string path = $"Assets/_Game/Prefabs/Levels/Level_{id:000}.prefab";
            GameObject prefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefabObj == null) return;

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabObj);
            
            // Clear old children
            for (int i = instance.transform.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(instance.transform.GetChild(i).gameObject);
            }

            // Populate new portrait layout
            populate(instance);

            // Add bounds
            AddBounds(instance);

            PrefabUtility.SaveAsPrefabAsset(instance, path);
            GameObject.DestroyImmediate(instance);
        }

        private static void AddBounds(GameObject root)
        {
            var lb = root.GetComponent<LevelBounds>();
            if (lb == null) lb = root.AddComponent<LevelBounds>();

            float minX = 0, maxX = 0, minY = 0, maxY = 0;
            bool first = true;

            foreach (Transform child in root.transform)
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

            // Add a little margin directly to the bounds so the camera framing is guaranteed to not cut off object edges.
            minX -= 0.5f; maxX += 0.5f;
            minY -= 0.5f; maxY += 0.5f;

            lb.center = new Vector2((minX + maxX) / 2f, (minY + maxY) / 2f);
            lb.size = new Vector2(maxX - minX, maxY - minY);
        }

        private static GameObject SpawnPlatform(GameObject parent, Vector3 pos, Vector2 size)
        {
            var obj = Spawn(parent, "StaticPlatform", pos, size);
            var sr = obj.GetComponent<SpriteRenderer>();
            sr.color = new Color(0.2f, 0.2f, 0.2f); // Dark charcoal
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
    }
}
