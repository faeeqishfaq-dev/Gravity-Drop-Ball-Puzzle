using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using PhysicsDraw.InputSystem;

namespace PhysicsDraw.Testing
{
    public class AutomatedPlayTest : MonoBehaviour
    {
        private IEnumerator Start()
        {
            Debug.Log("[AutomatedPlayTest] Starting End-to-End Test...");
            
            // Wait for Level 1 to load
            yield return new WaitForSeconds(1f);

            var ball = FindObjectOfType<PhysicsDraw.Physics.BallController>();
            if (ball != null)
            {
                Debug.Log($"[AutomatedPlayTest] Ball found at {ball.transform.position}. Simulated: {ball.GetComponent<Rigidbody2D>().simulated}");
            }
            else
            {
                Debug.LogError("[AutomatedPlayTest] Ball not found!");
            }

            Debug.Log("[AutomatedPlayTest] Simulating Draw...");
            
            // Simulate drawing a ramp
            var input = InputProvider.Instance;
            if (input == null)
            {
                Debug.LogError("[AutomatedPlayTest] InputProvider missing.");
                yield break;
            }

            Camera cam = Camera.main;
            Vector2 startScreen = cam.WorldToScreenPoint(new Vector3(-4, 2, 0));
            Vector2 endScreen = cam.WorldToScreenPoint(new Vector3(4, -2, 0));

            input.SimulatePointerDown(startScreen);
            yield return new WaitForSeconds(0.1f);
            input.SimulatePointerMove(Vector2.Lerp(startScreen, endScreen, 0.5f));
            yield return new WaitForSeconds(0.1f);
            input.SimulatePointerUp(endScreen);

            Debug.Log("[AutomatedPlayTest] Draw simulated. Waiting for ball to fall...");
            
            yield return new WaitForSeconds(3f);

            if (PhysicsDraw.Core.GameManager.Instance.CurrentState == PhysicsDraw.Core.GameState.Success)
            {
                Debug.Log("[AutomatedPlayTest] SUCCESS! Level 1 completed automatically.");
            }
            else
            {
                Debug.LogError($"[AutomatedPlayTest] FAILED! State is {PhysicsDraw.Core.GameManager.Instance.CurrentState}");
            }
        }
    }
}
