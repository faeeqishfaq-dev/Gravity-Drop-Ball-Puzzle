using UnityEditor;
using UnityEngine;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase5Setup
    {
        static Phase5Setup()
        {
            if (!SessionState.GetBool("Phase5SetupDone", false))
            {
                EditorApplication.delayCall += RunSetup;
                SessionState.SetBool("Phase5SetupDone", true);
            }
        }

        [MenuItem("PhysicsDraw/Run Phase 5 Setup")]
        public static void RunSetup()
        {
            CreateMechanicPrefabs();
            Debug.Log("[Phase5Setup] Phase 5 Setup Complete.");
        }

        private static void CreateMechanicPrefabs()
        {
            string prefabPath = "Assets/_Game/Prefabs/Mechanics";
            if (!AssetDatabase.IsValidFolder("Assets/_Game/Prefabs/Mechanics"))
            {
                AssetDatabase.CreateFolder("Assets/_Game/Prefabs", "Mechanics");
            }

            CreateMovingPlatform(prefabPath);
            CreateGate(prefabPath);
            CreateButton(prefabPath);
            CreatePressurePlate(prefabPath);
            CreateSpring(prefabPath);
            CreateConveyor(prefabPath);
        }

        private static void CreateMovingPlatform(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path + "/MovingPlatform.prefab") == null)
            {
                GameObject obj = new GameObject("MovingPlatform");
                obj.layer = LayerMask.NameToLayer("DynamicEnvironment");
                var sr = obj.AddComponent<SpriteRenderer>();
                sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
                sr.color = new Color(1f, 0.5f, 0f); // Orange
                sr.drawMode = SpriteDrawMode.Sliced;
                sr.size = new Vector2(3, 0.5f);
                obj.AddComponent<BoxCollider2D>().size = new Vector2(3, 0.5f);
                obj.AddComponent<PhysicsDraw.Mechanics.MovingPlatform>();
                PrefabUtility.SaveAsPrefabAsset(obj, path + "/MovingPlatform.prefab");
                GameObject.DestroyImmediate(obj);
            }
        }

        private static void CreateGate(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path + "/Gate.prefab") == null)
            {
                GameObject obj = new GameObject("Gate");
                obj.layer = LayerMask.NameToLayer("DynamicEnvironment");
                var sr = obj.AddComponent<SpriteRenderer>();
                sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
                sr.color = new Color(1f, 0.5f, 0f); // Orange
                sr.drawMode = SpriteDrawMode.Sliced;
                sr.size = new Vector2(0.5f, 3f);
                obj.AddComponent<BoxCollider2D>().size = new Vector2(0.5f, 3f);
                obj.AddComponent<PhysicsDraw.Mechanics.Gate>();
                PrefabUtility.SaveAsPrefabAsset(obj, path + "/Gate.prefab");
                GameObject.DestroyImmediate(obj);
            }
        }

        private static void CreateButton(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path + "/Button.prefab") == null)
            {
                GameObject root = new GameObject("Button");
                root.layer = LayerMask.NameToLayer("Trigger");

                GameObject visual = new GameObject("Visual");
                visual.transform.SetParent(root.transform);
                visual.transform.localPosition = new Vector3(0, 0.25f, 0);
                
                var sr = visual.AddComponent<SpriteRenderer>();
                sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
                sr.color = Color.yellow;
                sr.drawMode = SpriteDrawMode.Sliced;
                sr.size = new Vector2(1f, 0.5f);

                var col = root.AddComponent<BoxCollider2D>();
                col.size = new Vector2(1f, 0.5f);
                col.offset = new Vector2(0, 0.25f);

                root.AddComponent<PhysicsDraw.Mechanics.PhysicsButton>();

                PrefabUtility.SaveAsPrefabAsset(root, path + "/Button.prefab");
                GameObject.DestroyImmediate(root);
            }
        }

        private static void CreatePressurePlate(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path + "/PressurePlate.prefab") == null)
            {
                GameObject root = new GameObject("PressurePlate");
                root.layer = LayerMask.NameToLayer("Trigger");

                GameObject visual = new GameObject("Visual");
                visual.transform.SetParent(root.transform);
                visual.transform.localPosition = new Vector3(0, 0.1f, 0);
                
                var sr = visual.AddComponent<SpriteRenderer>();
                sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
                sr.color = Color.yellow;
                sr.drawMode = SpriteDrawMode.Sliced;
                sr.size = new Vector2(2f, 0.2f);

                var col = root.AddComponent<BoxCollider2D>();
                col.size = new Vector2(2f, 0.5f);
                col.offset = new Vector2(0, 0.25f);
                col.isTrigger = true;

                root.AddComponent<PhysicsDraw.Mechanics.PressurePlate>();

                PrefabUtility.SaveAsPrefabAsset(root, path + "/PressurePlate.prefab");
                GameObject.DestroyImmediate(root);
            }
        }

        private static void CreateSpring(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path + "/Spring.prefab") == null)
            {
                GameObject obj = new GameObject("Spring");
                obj.layer = LayerMask.NameToLayer("StaticEnvironment");
                var sr = obj.AddComponent<SpriteRenderer>();
                sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
                sr.color = Color.magenta;
                sr.drawMode = SpriteDrawMode.Sliced;
                sr.size = new Vector2(1f, 0.5f);
                obj.AddComponent<BoxCollider2D>().size = new Vector2(1f, 0.5f);
                
                var spring = obj.AddComponent<PhysicsDraw.Mechanics.Spring>();
                spring.launchDirection = Vector2.up;
                
                PrefabUtility.SaveAsPrefabAsset(obj, path + "/Spring.prefab");
                GameObject.DestroyImmediate(obj);
            }
        }

        private static void CreateConveyor(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path + "/Conveyor.prefab") == null)
            {
                GameObject obj = new GameObject("Conveyor");
                obj.layer = LayerMask.NameToLayer("StaticEnvironment");
                var sr = obj.AddComponent<SpriteRenderer>();
                sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
                sr.color = new Color(0.8f, 0.8f, 0.8f);
                sr.drawMode = SpriteDrawMode.Sliced;
                sr.size = new Vector2(4f, 0.5f);

                var col = obj.AddComponent<BoxCollider2D>();
                col.size = new Vector2(4f, 0.5f);
                col.usedByEffector = true;

                obj.AddComponent<SurfaceEffector2D>();
                obj.AddComponent<PhysicsDraw.Mechanics.Conveyor>();
                
                PrefabUtility.SaveAsPrefabAsset(obj, path + "/Conveyor.prefab");
                GameObject.DestroyImmediate(obj);
            }
        }
    }
}
