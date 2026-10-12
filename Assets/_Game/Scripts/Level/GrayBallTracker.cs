using UnityEngine;
using System.Collections.Generic;

namespace PhysicsDraw.Level
{
    public class GrayBallTracker : MonoBehaviour
    {
        private HashSet<GrayBall> enteredBalls = new HashSet<GrayBall>();
        private int totalGrayBalls = 0;

        private void Start()
        {
            totalGrayBalls = FindObjectsOfType<GrayBall>().Length;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var gb = collision.GetComponent<GrayBall>();
            if (gb != null && !gb.hasEnteredGoal)
            {
                gb.hasEnteredGoal = true;
                enteredBalls.Add(gb);

                if (enteredBalls.Count >= totalGrayBalls && totalGrayBalls > 0)
                {
                    // Fail the level
                    if (Core.GameManager.Instance != null && Core.GameManager.Instance.CurrentState != Core.GameState.Success)
                    {
                        Core.GameManager.Instance.ChangeState(Core.GameState.Failed);
                    }
                }
            }
        }
    }
}
