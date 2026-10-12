using UnityEngine;

namespace PhysicsDraw.Core
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        private Camera cam;

        [Header("Padding (World Units)")]
        [Tooltip("Extra space at top for HUD")]
        public float topPadding = 3f;
        [Tooltip("Extra space at bottom for HUD")]
        public float bottomPadding = 4f;
        [Tooltip("Extra space on sides")]
        public float horizontalPadding = 1f;

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        public void FrameLevel(PhysicsDraw.Level.LevelBounds levelBounds)
        {
            if (cam == null) cam = GetComponent<Camera>();
            if (levelBounds == null) return;

            Bounds b = levelBounds.GetBounds();

            // Total required size includes the world bounds plus our UI padding
            float requiredWidth = b.size.x + horizontalPadding * 2;
            float requiredHeight = b.size.y + topPadding + bottomPadding;

            // Calculate target orthographic size
            float sizeFromHeight = requiredHeight / 2f;
            float sizeFromWidth = requiredWidth / (2f * cam.aspect);

            float finalOrthoSize = Mathf.Max(sizeFromHeight, sizeFromWidth);

            cam.orthographicSize = finalOrthoSize;

            // Calculate camera center
            // The bounds center is b.center.
            // But we have asymmetrical padding (top vs bottom).
            // If topPadding is 3 and bottomPadding is 1, the total vertical padding is 4.
            // The "visual" center of the screen in world space will be shifted.
            // To keep the 'b.center' at the true center of the safe area, we must offset the camera.
            // The camera center is: b.center + (topPadding - bottomPadding) / 2
            float yOffset = (topPadding - bottomPadding) / 2f;

            cam.transform.position = new Vector3(b.center.x, b.center.y + yOffset, -10f);

            CreateSideWalls();

            Debug.Log($"[CameraController] Framed Level. Size: {finalOrthoSize}, Pos: {cam.transform.position}");
        }

        private void CreateSideWalls()
        {
            float halfHeight = cam.orthographicSize;
            float halfWidth = cam.orthographicSize * cam.aspect;

            // Find or create walls container
            Transform wallsContainer = transform.Find("ScreenBoundsWalls");
            if (wallsContainer == null)
            {
                GameObject wc = new GameObject("ScreenBoundsWalls");
                wc.transform.SetParent(transform);
                wc.transform.localPosition = Vector3.zero;
                wallsContainer = wc.transform;
            }

            // Clear old walls
            foreach (Transform child in wallsContainer)
            {
                Destroy(child.gameObject);
            }

            // Left Wall
            CreateWall(wallsContainer, "LeftWall", new Vector2(-halfWidth - 0.5f, 0), new Vector2(1f, halfHeight * 2f + 10f));
            // Right Wall
            CreateWall(wallsContainer, "RightWall", new Vector2(halfWidth + 0.5f, 0), new Vector2(1f, halfHeight * 2f + 10f));
        }

        private void CreateWall(Transform parent, string name, Vector2 localPos, Vector2 size)
        {
            GameObject wall = new GameObject(name);
            wall.transform.SetParent(parent);
            wall.transform.localPosition = new Vector3(localPos.x, localPos.y, 0);
            
            BoxCollider2D bc = wall.AddComponent<BoxCollider2D>();
            bc.size = size;
            
            // Assign a physics material if needed, or let it use default
            wall.layer = LayerMask.NameToLayer("Default");
        }
    }
}
