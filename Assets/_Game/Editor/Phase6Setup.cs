using UnityEditor;
using UnityEngine;
using PhysicsDraw.Mechanics;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase6Setup
    {
        static Phase6Setup()
        {
            if (!SessionState.GetBool("Phase6SetupDone", false))
            {
                EditorApplication.delayCall += RunSetup;
                SessionState.SetBool("Phase6SetupDone", true);
            }
        }

        [MenuItem("PhysicsDraw/Run Phase 6 Setup")]
        public static void RunSetup()
        {
            CreateMechanicPrefabs();
            Debug.Log("[Phase6Setup] Phase 6 Setup Complete.");
        }

        private static void CreateMechanicPrefabs()
        {
            string prefabPath = "Assets/_Game/Prefabs/Mechanics";
            
            CreateGravityZone(prefabPath);
            CreateGravitySwitch(prefabPath);
            CreateFan(prefabPath);
            CreatePortal(prefabPath);
            CreateHazard(prefabPath);

            InjectGravityManager();
        }

        private static void InjectGravityManager()
        {
            string scenePath = "Assets/_Game/Scenes/TestPhysics.unity";
            if (System.IO.File.Exists(scenePath))
            {
                var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);
                var gmObj = GameObject.Find("GameManager");
                if (gmObj != null && gmObj.GetComponent<Core.GravityManager>() == null)
                {
                    gmObj.AddComponent<Core.GravityManager>();
                }
                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene, scenePath);
            }
        }

        private static void CreateGravityZone(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path + "/GravityZone.prefab") == null)
            {
                GameObject obj = new GameObject("GravityZone");
                obj.layer = LayerMask.NameToLayer("Trigger");
                var col = obj.AddComponent<BoxCollider2D>();
                col.isTrigger = true;
                col.size = new Vector2(4, 4);
                obj.AddComponent<GravityZone>();
                PrefabUtility.SaveAsPrefabAsset(obj, path + "/GravityZone.prefab");
                GameObject.DestroyImmediate(obj);
            }
        }

        private static void CreateGravitySwitch(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path + "/GravitySwitch.prefab") == null)
            {
                GameObject obj = new GameObject("GravitySwitch");
                obj.AddComponent<GravitySwitch>();
                PrefabUtility.SaveAsPrefabAsset(obj, path + "/GravitySwitch.prefab");
                GameObject.DestroyImmediate(obj);
            }
        }

        private static void CreateFan(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path + "/Fan.prefab") == null)
            {
                GameObject obj = new GameObject("Fan");
                obj.layer = LayerMask.NameToLayer("Trigger");
                var sr = obj.AddComponent<SpriteRenderer>();
                sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
                sr.color = new Color(0.8f, 0.9f, 1f, 0.5f);
                sr.drawMode = SpriteDrawMode.Sliced;
                sr.size = new Vector2(2, 4);

                var col = obj.AddComponent<BoxCollider2D>();
                col.isTrigger = true;
                col.size = new Vector2(2, 4);
                col.offset = new Vector2(0, 2);

                var effector = obj.AddComponent<AreaEffector2D>();
                obj.AddComponent<Fan>();
                
                PrefabUtility.SaveAsPrefabAsset(obj, path + "/Fan.prefab");
                GameObject.DestroyImmediate(obj);
            }
        }

        private static void CreatePortal(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path + "/Portal.prefab") == null)
            {
                GameObject obj = new GameObject("Portal");
                obj.layer = LayerMask.NameToLayer("Trigger");
                var sr = obj.AddComponent<SpriteRenderer>();
                sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
                sr.color = new Color(0.5f, 0f, 1f); // Purple

                var col = obj.AddComponent<CircleCollider2D>();
                col.isTrigger = true;
                
                GameObject exitObj = new GameObject("ExitPoint");
                exitObj.transform.SetParent(obj.transform);
                exitObj.transform.localPosition = new Vector3(0, 1, 0);

                var portal = obj.AddComponent<Portal>();
                portal.exitPoint = exitObj.transform;

                PrefabUtility.SaveAsPrefabAsset(obj, path + "/Portal.prefab");
                GameObject.DestroyImmediate(obj);
            }
        }

        private static void CreateHazard(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path + "/Spikes.prefab") == null)
            {
                GameObject obj = new GameObject("Spikes");
                obj.layer = LayerMask.NameToLayer("Hazard");
                var sr = obj.AddComponent<SpriteRenderer>();
                sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
                sr.color = Color.red;
                sr.drawMode = SpriteDrawMode.Sliced;
                sr.size = new Vector2(2, 0.5f);

                var col = obj.AddComponent<BoxCollider2D>();
                col.size = new Vector2(2, 0.5f);

                obj.AddComponent<Hazard>();

                PrefabUtility.SaveAsPrefabAsset(obj, path + "/Spikes.prefab");
                GameObject.DestroyImmediate(obj);
            }
        }
    }
}
