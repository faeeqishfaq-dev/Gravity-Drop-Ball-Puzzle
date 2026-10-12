using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.IO;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class ProjectSetup
    {
        static ProjectSetup()
        {
            if (!SessionState.GetBool("Phase1SetupDone", false))
            {
                EditorApplication.delayCall += RunSetup;
                SessionState.SetBool("Phase1SetupDone", true);
            }
        }

        [MenuItem("PhysicsDraw/Run Phase 1 Setup")]
        public static void RunSetup()
        {
            SetupLayers();
            CreateSettingsAssets();
            CreateScenes();
            Debug.Log("[ProjectSetup] Phase 1 Setup Complete.");
        }

        private static void SetupLayers()
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layers = tagManager.FindProperty("layers");

            string[] customLayers = { "Ball", "Drawing", "StaticEnvironment", "DynamicEnvironment", "Goal", "Hazard", "Trigger", "Mechanism", "UI" };
            
            for (int j = 0; j < customLayers.Length; j++)
            {
                string layerName = customLayers[j];
                bool layerExists = false;
                for (int i = 8; i < layers.arraySize; i++)
                {
                    SerializedProperty layerSp = layers.GetArrayElementAtIndex(i);
                    if (layerSp.stringValue == layerName)
                    {
                        layerExists = true;
                        break;
                    }
                }

                if (!layerExists)
                {
                    for (int i = 8; i < layers.arraySize; i++)
                    {
                        SerializedProperty layerSp = layers.GetArrayElementAtIndex(i);
                        if (string.IsNullOrEmpty(layerSp.stringValue))
                        {
                            layerSp.stringValue = layerName;
                            break;
                        }
                    }
                }
            }

            tagManager.ApplyModifiedProperties();
        }

        private static void CreateSettingsAssets()
        {
            if (!AssetDatabase.IsValidFolder("Assets/_Game/Resources"))
            {
                AssetDatabase.CreateFolder("Assets/_Game", "Resources");
            }
            if (!AssetDatabase.IsValidFolder("Assets/_Game/Resources/Settings"))
            {
                AssetDatabase.CreateFolder("Assets/_Game/Resources", "Settings");
            }

            if (AssetDatabase.LoadAssetAtPath<PhysicsDraw.Core.GameplaySettings>("Assets/_Game/Resources/Settings/GameplaySettings.asset") == null)
            {
                var gs = ScriptableObject.CreateInstance<PhysicsDraw.Core.GameplaySettings>();
                AssetDatabase.CreateAsset(gs, "Assets/_Game/Resources/Settings/GameplaySettings.asset");
            }

            if (AssetDatabase.LoadAssetAtPath<PhysicsDraw.Core.PhysicsSettings>("Assets/_Game/Resources/Settings/PhysicsSettings.asset") == null)
            {
                var ps = ScriptableObject.CreateInstance<PhysicsDraw.Core.PhysicsSettings>();
                AssetDatabase.CreateAsset(ps, "Assets/_Game/Resources/Settings/PhysicsSettings.asset");
            }

            AssetDatabase.SaveAssets();
        }

        private static void CreateScenes()
        {
            string scenePath = "Assets/_Game/Scenes/";
            string[] sceneNames = { "Bootstrap", "MainMenu", "Gameplay" };

            var originalScenes = EditorBuildSettings.scenes;
            var newScenes = new System.Collections.Generic.List<EditorBuildSettingsScene>();
            bool addedNewScenes = false;

            foreach (string name in sceneNames)
            {
                string fullPath = scenePath + name + ".unity";
                if (!File.Exists(fullPath))
                {
                    UnityEngine.SceneManagement.Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                    
                    if (name == "Bootstrap")
                    {
                        GameObject coreManager = new GameObject("CoreManagers");
                        coreManager.AddComponent<PhysicsDraw.Core.GameManager>();
                        coreManager.AddComponent<PhysicsDraw.Save.SaveManager>();
                        coreManager.AddComponent<PhysicsDraw.InputSystem.InputProvider>();
                        coreManager.AddComponent<PhysicsDraw.Core.GameBootstrap>();
                        
                        GameObject mainCamera = new GameObject("Main Camera");
                        mainCamera.AddComponent<Camera>().orthographic = true;
                        mainCamera.tag = "MainCamera";
                    }

                    EditorSceneManager.SaveScene(newScene, fullPath);
                    addedNewScenes = true;
                }
                newScenes.Add(new EditorBuildSettingsScene(fullPath, true));
            }
            
            if (addedNewScenes)
            {
                EditorBuildSettings.scenes = newScenes.ToArray();
            }
        }
    }
}
