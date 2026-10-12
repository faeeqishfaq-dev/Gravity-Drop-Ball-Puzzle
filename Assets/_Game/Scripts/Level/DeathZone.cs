using UnityEngine;
using PhysicsDraw.Core;

namespace PhysicsDraw.Level
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") || collision.name == "Ball")
            {
                if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameState.Playing)
                {
                    Debug.Log("[DeathZone] Ball fell out of bounds!");
                    GameManager.Instance.ChangeState(GameState.Failed);
                }
            }
        }
    }
}
