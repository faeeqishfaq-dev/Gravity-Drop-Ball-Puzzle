using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase2Setup
    {
        static Phase2Setup()
        {
            if (!SessionState.GetBool("Phase2SetupDone", false))
            {
                EditorApplication.delayCall += RunSetup;
                SessionState.SetBool("Phase2SetupDone", true);
            }
        }

        [MenuItem("PhysicsDraw/Run Phase 2 Setup")]
        public static void RunSetup()
        {
            CreatePhysicsMaterials();
            CreatePrefabs();
            CreateTestScene();
            Debug.Log("[Phase2Setup] Phase 2 Setup Complete.");
        }

        private static void CreatePhysicsMaterials()
        {
            string path = "Assets/_Game/Resources/PhysicsMaterials";
            if (!AssetDatabase.IsValidFolder("Assets/_Game/Resources"))
            {
                AssetDatabase.CreateFolder("Assets/_Game", "Resources");
            }
            if (!AssetDatabase.IsValidFolder("Assets/_Game/Resources/PhysicsMaterials"))
            {
                AssetDatabase.CreateFolder("Assets/_Game/Resources", "PhysicsMaterials");
            }

            CreateMaterial(path + "/Normal.physicsMaterial2D", 0.4f, 0.0f);
            CreateMaterial(path + "/Ball.physicsMaterial2D", 0.4f, 0.2f);
            CreateMaterial(path + "/Drawing.physicsMaterial2D", 0.6f, 0.0f);
            CreateMaterial(path + "/Rubber.physicsMaterial2D", 0.6f, 0.8f);
            CreateMaterial(path + "/Ice.physicsMaterial2D", 0.05f, 0.0f);
            CreateMaterial(path + "/Sticky.physicsMaterial2D", 1.0f, 0.0f);
        }

        private static void CreateMaterial(string path, float friction, float bounciness)
        {
            if (AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>(path) == null)
            {
                PhysicsMaterial2D mat = new PhysicsMaterial2D();
                mat.friction = friction;
                mat.bounciness = bounciness;
                AssetDatabase.CreateAsset(mat, path);
            }
        }

        private static void CreatePrefabs()
        {
            string prefabPath = "Assets/_Game/Prefabs";
            if (!AssetDatabase.IsValidFolder(prefabPath))
            {
                AssetDatabase.CreateFolder("Assets/_Game", "Prefabs");
            }

            // Ball
            if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/Ball.prefab") == null)
            {
                GameObject ball = new GameObject("Ball");
                ball.layer = LayerMask.NameToLayer("Ball");
                
                SpriteRenderer sr = ball.AddComponent<SpriteRenderer>();
                sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
                sr.color = Color.red;

                Rigidbody2D rb = ball.AddComponent<Rigidbody2D>();
                rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                rb.sharedMaterial = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>("Assets/_Game/Resources/PhysicsMaterials/Ball.physicsMaterial2D");

                ball.AddComponent<CircleCollider2D>();
                ball.AddComponent<PhysicsDraw.Physics.BallController>();

                PrefabUtility.SaveAsPrefabAsset(ball, prefabPath + "/Ball.prefab");
                GameObject.DestroyImmediate(ball);
            }

            // Static Platform
            if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/StaticPlatform.prefab") == null)
            {
                GameObject platform = new GameObject("StaticPlatform");
                platform.layer = LayerMask.NameToLayer("StaticEnvironment");
                
                SpriteRenderer sr = platform.AddComponent<SpriteRenderer>();
                sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
                sr.color = Color.gray;
                sr.drawMode = SpriteDrawMode.Sliced;
                sr.size = new Vector2(5, 1);

                BoxCollider2D col = platform.AddComponent<BoxCollider2D>();
                col.size = new Vector2(5, 1);

                PrefabUtility.SaveAsPrefabAsset(platform, prefabPath + "/StaticPlatform.prefab");
                GameObject.DestroyImmediate(platform);
            }

            // Goal
            if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/Goal.prefab") == null)
            {
                GameObject goalRoot = new GameObject("Goal");
                
                GameObject leftWall = CreateGoalWall("LeftWall", new Vector2(-0.4f, 0), new Vector2(0.2f, 1f));
                leftWall.transform.SetParent(goalRoot.transform);
                
                GameObject rightWall = CreateGoalWall("RightWall", new Vector2(0.4f, 0), new Vector2(0.2f, 1f));
                rightWall.transform.SetParent(goalRoot.transform);
                
                GameObject bottomWall = CreateGoalWall("BottomWall", new Vector2(0, -0.4f), new Vector2(1f, 0.2f));
                bottomWall.transform.SetParent(goalRoot.transform);

                GameObject successTrigger = new GameObject("SuccessTrigger");
                successTrigger.transform.SetParent(goalRoot.transform);
                successTrigger.transform.localPosition = new Vector3(0, 0, 0);
                BoxCollider2D triggerCol = successTrigger.AddComponent<BoxCollider2D>();
                triggerCol.isTrigger = true;
                triggerCol.size = new Vector2(0.6f, 0.6f);
                successTrigger.AddComponent<PhysicsDraw.Level.Goal>();

                PrefabUtility.SaveAsPrefabAsset(goalRoot, prefabPath + "/Goal.prefab");
                GameObject.DestroyImmediate(goalRoot);
            }

            // OutOfBounds (DeathZone)
            if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/DeathZone.prefab") == null)
            {
                GameObject deathZone = new GameObject("DeathZone");
                deathZone.layer = LayerMask.NameToLayer("Hazard");
                BoxCollider2D col = deathZone.AddComponent<BoxCollider2D>();
                col.isTrigger = true;
                col.size = new Vector2(50, 10);
                deathZone.AddComponent<PhysicsDraw.Level.OutOfBounds>();
                
                PrefabUtility.SaveAsPrefabAsset(deathZone, prefabPath + "/DeathZone.prefab");
                GameObject.DestroyImmediate(deathZone);
            }
        }

        private static GameObject CreateGoalWall(string name, Vector2 pos, Vector2 size)
        {
            GameObject wall = new GameObject(name);
            wall.layer = LayerMask.NameToLayer("Goal");
            wall.transform.localPosition = pos;
            
            SpriteRenderer sr = wall.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            sr.color = Color.green;
            sr.drawMode = SpriteDrawMode.Sliced;
            sr.size = size;

            BoxCollider2D col = wall.AddComponent<BoxCollider2D>();
            col.size = size;
            
            return wall;
        }

        private static void CreateTestScene()
        {
            string scenePath = "Assets/_Game/Scenes/TestPhysics.unity";
            if (!File.Exists(scenePath))
            {
                UnityEngine.SceneManagement.Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                
                GameObject mainCamera = new GameObject("Main Camera");
                Camera cam = mainCamera.AddComponent<Camera>();
                cam.orthographic = true;
                cam.orthographicSize = 10;
                mainCamera.tag = "MainCamera";
                mainCamera.transform.position = new Vector3(0, 0, -10);

                // Add platform
                GameObject platform = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Prefabs/StaticPlatform.prefab")) as GameObject;
                if (platform != null) platform.transform.position = new Vector3(0, -3, 0);

                // Add Ball
                GameObject ball = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Prefabs/Ball.prefab")) as GameObject;
                if (ball != null) ball.transform.position = new Vector3(-2, 3, 0);

                // Add Goal
                GameObject goal = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Prefabs/Goal.prefab")) as GameObject;
                if (goal != null) goal.transform.position = new Vector3(2, -2.5f, 0);

                // Add DeathZone
                GameObject deathZone = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Prefabs/DeathZone.prefab")) as GameObject;
                if (deathZone != null) deathZone.transform.position = new Vector3(0, -15, 0);

                EditorSceneManager.SaveScene(newScene, scenePath);
                
                var originalScenes = EditorBuildSettings.scenes;
                var newScenes = new System.Collections.Generic.List<EditorBuildSettingsScene>(originalScenes);
                newScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                EditorBuildSettings.scenes = newScenes.ToArray();
            }
        }
    }
}
