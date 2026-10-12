using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase3Setup
    {
        static Phase3Setup()
        {
            if (!SessionState.GetBool("Phase3SetupDone", false))
            {
                EditorApplication.delayCall += RunSetup;
                SessionState.SetBool("Phase3SetupDone", true);
            }
        }

        [MenuItem("PhysicsDraw/Run Phase 3 Setup")]
        public static void RunSetup()
        {
            SetupDrawingInTestScene();
            Debug.Log("[Phase3Setup] Phase 3 Setup Complete.");
        }

        private static void SetupDrawingInTestScene()
        {
            string scenePath = "Assets/_Game/Scenes/TestPhysics.unity";
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            // Check if DrawingController exists
            var drawingController = GameObject.FindObjectOfType<PhysicsDraw.Drawing.DrawingController>();
            if (drawingController == null)
            {
                GameObject dcObj = new GameObject("DrawingController");
                dcObj.AddComponent<PhysicsDraw.Drawing.DrawingController>();
            }

            // Ensure GameManager is in the scene so State is Playing for drawing
            var gameManager = GameObject.FindObjectOfType<PhysicsDraw.Core.GameManager>();
            if (gameManager == null)
            {
                GameObject gmObj = new GameObject("GameManager");
                gameManager = gmObj.AddComponent<PhysicsDraw.Core.GameManager>();
                gmObj.AddComponent<PhysicsDraw.EditorScripts.TestSceneController>();
            }

            // Ensure InputProvider is in the scene
            var inputProvider = GameObject.FindObjectOfType<PhysicsDraw.InputSystem.InputProvider>();
            if (inputProvider == null)
            {
                GameObject ipObj = new GameObject("InputProvider");
                ipObj.AddComponent<PhysicsDraw.InputSystem.InputProvider>();
            }

            EditorSceneManager.SaveScene(scene, scenePath);
        }
    }
}
