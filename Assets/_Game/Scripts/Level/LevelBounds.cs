using UnityEngine;

namespace PhysicsDraw.Level
{
    public class LevelBounds : MonoBehaviour
    {
        [Header("Define Playable Area")]
        public Vector2 center = Vector2.zero;
        public Vector2 size = new Vector2(12, 16);

        public Bounds GetBounds()
        {
            return new Bounds(transform.position + (Vector3)center, size);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawWireCube(transform.position + (Vector3)center, size);
        }
    }
}
