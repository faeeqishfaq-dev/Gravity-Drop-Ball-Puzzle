using UnityEngine;

namespace PhysicsDraw.Core
{
    [CreateAssetMenu(fileName = "PhysicsSettings", menuName = "PhysicsDraw/Settings/Physics Settings")]
    public class PhysicsSettings : ScriptableObject
    {
        [Header("Global Gravity")]
        public Vector2 defaultGravity = new Vector2(0, -9.81f);

        [Header("Ball Physics")]
        public float ballMass = 1.0f;
        public float ballDrag = 0.05f;
        public float ballAngularDrag = 0.05f;
        public float ballGravityScale = 1.0f;

        [Header("Drawing Physics")]
        public float drawingWidth = 0.1f;
        public float drawingGravityScale = 1.0f;
        [Tooltip("Mass assigned per unit of world length.")]
        public float massPerUnitLength = 1.5f;
        public float minimumDrawingMass = 0.5f;
        public float maximumDrawingMass = 10.0f;

        [Header("Drawing Optimization")]
        [Tooltip("Minimum distance required before adding a new point while drawing.")]
        public float minPointDistance = 0.05f;
        [Tooltip("Tolerance for stroke simplification when pointer is released.")]
        public float simplificationTolerance = 0.02f;
    }
}
