using UnityEditor;
using UnityEngine;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase12_FixPhysicsMaterials
    {
        static Phase12_FixPhysicsMaterials()
        {
            if (!SessionState.GetBool("Phase12_Done", false))
            {
                SessionState.SetBool("Phase12_Done", true);
                EditorApplication.delayCall += FixMaterials;
            }
        }

        [MenuItem("PhysicsDraw/Phase 12 - Fix Physics Materials")]
        public static void FixMaterials()
        {
            var drawingMat = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>("Assets/_Game/Resources/PhysicsMaterials/Drawing.physicsMaterial2D");
            if (drawingMat != null)
            {
                drawingMat.friction = 0.1f;
                drawingMat.bounciness = 0.1f;
                EditorUtility.SetDirty(drawingMat);
            }

            var ballMat = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>("Assets/_Game/Resources/PhysicsMaterials/Ball.physicsMaterial2D");
            if (ballMat != null)
            {
                ballMat.friction = 0.2f;
                ballMat.bounciness = 0.2f;
                EditorUtility.SetDirty(ballMat);
            }

            var settings = Resources.Load<Core.PhysicsSettings>("Settings/PhysicsSettings");
            if (settings != null)
            {
                settings.minPointDistance = 0.1f; // Smoother lines
                settings.simplificationTolerance = 0.05f;
                EditorUtility.SetDirty(settings);
            }

            AssetDatabase.SaveAssets();
            Debug.Log("[Phase12] Fixed Physics Materials for smoother rolling.");
        }
    }
}
