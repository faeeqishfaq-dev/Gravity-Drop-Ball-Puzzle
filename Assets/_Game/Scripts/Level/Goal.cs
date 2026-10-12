using System;
using UnityEngine;

namespace PhysicsDraw.Level
{
    public class Goal : MonoBehaviour
    {
        [Tooltip("How long the ball must remain inside to win. Overrides global setting if > 0.")]
        [SerializeField] private float overrideValidationTime = 0f;

        public event Action OnGoalAchieved;

        private bool isBallInside = false;
        private float timeInside = 0f;
        private bool hasWon = false;

        private float GetValidationTime()
        {
            if (overrideValidationTime > 0) return overrideValidationTime;
            
            // Basic fallback, ideally injected via GameManager/Settings
            return 0.3f; 
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (hasWon) return;
            if (Core.GameManager.Instance != null && Core.GameManager.Instance.CurrentState == Core.GameState.Failed) return;

            if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
            {
                if (collision.gameObject.GetComponent<GrayBall>() != null) return; // IGNORE GRAY BALLS
                isBallInside = true;
                timeInside = 0f;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (hasWon) return;

            if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
            {
                if (collision.gameObject.GetComponent<GrayBall>() != null) return; // IGNORE GRAY BALLS
                isBallInside = false;
                timeInside = 0f;
            }
        }

        private void Update()
        {
            if (hasWon || !isBallInside) return;

            timeInside += Time.deltaTime;
            if (timeInside >= GetValidationTime())
            {
                hasWon = true;
                Debug.Log("[Goal] Level Complete!");
                OnGoalAchieved?.Invoke();
                
                if (Core.GameManager.Instance != null)
                {
                    Core.GameManager.Instance.ChangeState(Core.GameState.Success);
                }
            }
        }
    }
}
