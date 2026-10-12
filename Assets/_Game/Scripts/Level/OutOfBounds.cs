using UnityEngine;

namespace PhysicsDraw.Level
{
    public class OutOfBounds : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
            {
                Debug.Log("[OutOfBounds] Ball fell out of bounds.");
                if (Core.GameManager.Instance != null)
                {
                    Core.GameManager.Instance.ChangeState(Core.GameState.Failed);
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Drawing"))
            {
                // Optionally destroy player drawings that fall out of bounds to save performance
                Destroy(collision.gameObject);
            }
        }
    }
}
