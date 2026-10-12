using UnityEngine;
using PhysicsDraw.Core;

namespace PhysicsDraw.Mechanics
{
    public class Hazard : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
            {
                Debug.Log("[Hazard] Ball destroyed by hazard.");
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ChangeState(GameState.Failed);
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Drawing"))
            {
                Destroy(collision.gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
            {
                Debug.Log("[Hazard] Ball hit hazard.");
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ChangeState(GameState.Failed);
                }
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Drawing"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
