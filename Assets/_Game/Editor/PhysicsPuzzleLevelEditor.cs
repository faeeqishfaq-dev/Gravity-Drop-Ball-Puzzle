using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.IO;
using PhysicsDraw.Level;
using PhysicsDraw.Physics;

namespace PhysicsDraw.EditorScripts
{
    public class PhysicsPuzzleLevelEditor : EditorWindow
    {
        private int newLevelID = 1;
        private string newLevelName = "New Level";
        private Vector2 scrollPos;

        [MenuItem("PhysicsDraw/Physics Puzzle Level Editor")]
        public static void ShowWindow()
        {
            GetWindow<PhysicsPuzzleLevelEditor>("Physics Puzzle Level Editor");
        }

        private void OnGUI()
        {
            GUILayout.Label("Level Authoring", EditorStyles.boldLabel);
            
            GUILayout.BeginHorizontal();
            newLevelID = EditorGUILayout.IntField("Level ID:", newLevelID);
            newLevelName = EditorGUILayout.TextField("Display Name:", newLevelName);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("1. Create New Level", GUILayout.Height(30)))
            {
                CreateNewLevel();
            }

            GUILayout.Space(15);
            GUILayout.Label("Level Population", EditorStyles.boldLabel);
            
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            if (GUILayout.Button("Add Ball Spawn")) InstantiateMechanic("Ball");
            if (GUILayout.Button("Add Goal")) InstantiateMechanic("Goal");
            if (GUILayout.Button("Add Platform")) InstantiateMechanic("StaticPlatform");
            if (GUILayout.Button("Add Moving Platform")) InstantiateMechanic("Mechanics/MovingPlatform");
            if (GUILayout.Button("Add Button")) InstantiateMechanic("Mechanics/Button");
            if (GUILayout.Button("Add Pressure Plate")) InstantiateMechanic("Mechanics/PressurePlate");
            if (GUILayout.Button("Add Gate")) InstantiateMechanic("Mechanics/Gate");
            if (GUILayout.Button("Add Bounce/Spring")) InstantiateMechanic("Mechanics/Spring");
            if (GUILayout.Button("Add Fan")) InstantiateMechanic("Mechanics/Fan");
            if (GUILayout.Button("Add Portal")) InstantiateMechanic("Mechanics/Portal");
            if (GUILayout.Button("Add Gravity Zone")) InstantiateMechanic("Mechanics/GravityZone");
            if (GUILayout.Button("Add Hazard (Spikes)")) InstantiateMechanic("Mechanics/Spikes");

            GUILayout.EndScrollView();

            GUILayout.Space(15);
            GUILayout.Label("Validation & Testing", EditorStyles.boldLabel);

            if (GUILayout.Button("2. Validate Current Level", GUILayout.Height(30)))
            {
                ValidateLevel();
            }

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("3. Play / Test Level", GUILayout.Height(40)))
            {
                TestLevel();
            }
            GUI.backgroundColor = Color.white;
        }

        private void CreateNewLevel()
        {
            string prefabPath = $"Assets/_Game/Prefabs/Levels/Level_{newLevelID:D3}.prefab";
            string dataPath = $"Assets/_Game/Resources/Levels/LevelData_{newLevelID:D3}.asset";

            if (!AssetDatabase.IsValidFolder("Assets/_Game/Prefabs/Levels"))
                AssetDatabase.CreateFolder("Assets/_Game/Prefabs", "Levels");
            
            if (!AssetDatabase.IsValidFolder("Assets/_Game/Resources/Levels"))
                AssetDatabase.CreateFolder("Assets/_Game/Resources", "Levels");

            // 1. Create Level Prefab
            if (AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath) == null)
            {
                GameObject levelRoot = new GameObject($"Level_{newLevelID:D3}");
                PrefabUtility.SaveAsPrefabAsset(levelRoot, prefabPath);
                DestroyImmediate(levelRoot);
            }

            // 2. Create Level Data
            LevelData data = AssetDatabase.LoadAssetAtPath<LevelData>(dataPath);
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<LevelData>();
                data.levelID = newLevelID;
                data.displayName = newLevelName;
                data.levelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                AssetDatabase.CreateAsset(data, dataPath);
            }

            // 3. Register to Database
            LevelDatabase db = Resources.Load<LevelDatabase>("LevelDatabase");
            if (db != null)
            {
                if (!db.allLevels.Contains(data))
                {
                    db.allLevels.Add(data);
                    EditorUtility.SetDirty(db);
                }
            }

            AssetDatabase.SaveAssets();

            // 4. Open the Prefab for editing
            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath));

            Debug.Log($"[LevelEditor] Successfully created and opened Level {newLevelID:D3}.");
            newLevelID++;
        }

        private void InstantiateMechanic(string prefabName)
        {
            string path = $"Assets/_Game/Prefabs/{prefabName}.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null)
            {
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                
                // If in prefab mode, parent to root. If in scene, just spawn.
                var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null)
                {
                    instance.transform.SetParent(prefabStage.prefabContentsRoot.transform, false);
                }
                else
                {
                    // Center in Scene view
                    SceneView view = SceneView.lastActiveSceneView;
                    if (view != null)
                    {
                        instance.transform.position = view.camera.transform.position + view.camera.transform.forward * 10f;
                        instance.transform.position = new Vector3(Mathf.Round(instance.transform.position.x), Mathf.Round(instance.transform.position.y), 0);
                    }
                }
                
                Selection.activeGameObject = instance;
                Undo.RegisterCreatedObjectUndo(instance, $"Add {prefabName}");
            }
            else
            {
                Debug.LogError($"[LevelEditor] Mechanic prefab not found at {path}");
            }
        }

        private void ValidateLevel()
        {
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            GameObject root = null;

            if (prefabStage != null)
            {
                root = prefabStage.prefabContentsRoot;
            }
            else
            {
                // Validate open scene if not in prefab mode
                root = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()[0]; 
            }

            bool hasBall = root.GetComponentsInChildren<BallController>(true).Length > 0;
            bool hasGoal = root.GetComponentsInChildren<Goal>(true).Length > 0;

            if (!hasBall) Debug.LogError("[Level Validation] ERROR: Missing Ball Spawn in level!");
            if (!hasGoal) Debug.LogError("[Level Validation] ERROR: Missing Goal in level!");
            
            if (hasBall && hasGoal)
            {
                Debug.Log("[Level Validation] SUCCESS: Level is valid.");
            }
        }

        private void TestLevel()
        {
            // If in prefab mode, save and exit
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                // Try to infer level ID from prefab name (e.g. Level_001)
                string name = prefabStage.prefabContentsRoot.name;
                string[] parts = name.Split('_');
                int idToTest = 1;
                if (parts.Length > 1 && int.TryParse(parts[1], out int parsed))
                {
                    idToTest = parsed;
                }

                // Close prefab mode
                UnityEditor.SceneManagement.StageUtility.GoToMainStage();

                // Open TestPhysics scene
                EditorSceneManager.OpenScene("Assets/_Game/Scenes/TestPhysics.unity");
                
                // Pass level ID to a testing bootstrapper via PlayerPrefs
                PlayerPrefs.SetInt("EditorTestLevelID", idToTest);
                EditorApplication.isPlaying = true;
            }
            else
            {
                EditorApplication.isPlaying = true;
            }
        }
    }
}
