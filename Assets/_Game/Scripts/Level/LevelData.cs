using UnityEngine;

namespace PhysicsDraw.Level
{
    public enum BallStartMode
    {
        Immediate,
        OnFirstDraw
    }

    [CreateAssetMenu(fileName = "LevelData", menuName = "PhysicsDraw/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("General")]
        public int levelID;
        public string displayName;
        public GameObject levelPrefab;
        public BallStartMode ballStartMode = BallStartMode.OnFirstDraw;

        [Header("Rules")]
        public bool drawingEnabled = true;
        public bool unlimitedDrawing = true;
        public float inkLimit = 100f;
        public int maxDrawings = 999;

        [Header("Physics")]
        public Vector2 gravityDirection = new Vector2(0, -9.81f);
        public float gravityStrength = 1f;

        [Header("Camera")]
        public Vector3 cameraPosition = new Vector3(0, 0, -10);
        public float orthographicSize = 10f;

        [Header("Stars")]
        public float threeStarInk = 10f;
        public float twoStarInk = 20f;
    }
}
