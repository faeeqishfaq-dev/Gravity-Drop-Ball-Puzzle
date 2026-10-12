using UnityEngine;

namespace PhysicsDraw.Physics
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class BallController : MonoBehaviour, Core.IResettable
    {
        private Rigidbody2D rb;
        private Vector3 startPosition;
        private Quaternion startRotation;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            startPosition = transform.position;
            startRotation = transform.rotation;
            
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        private void Start()
        {
            ApplyPhysicsSettings();
        }

        private void ApplyPhysicsSettings()
        {
            var settings = Resources.Load<Core.PhysicsSettings>("Settings/PhysicsSettings");
            if (settings != null)
            {
                rb.mass = settings.ballMass;
                rb.drag = settings.ballDrag;
                rb.angularDrag = settings.ballAngularDrag;
                rb.gravityScale = settings.ballGravityScale;
            }
        }

        public void ResetObject()
        {
            if (rb == null) return;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.position = startPosition;
            rb.rotation = startRotation.eulerAngles.z;
        }

        private void Update()
        {
            if (Core.GameManager.Instance != null)
            {
                var state = Core.GameManager.Instance.CurrentState;
                if (state == Core.GameState.Ready || state == Core.GameState.Paused || state == Core.GameState.Success || state == Core.GameState.Failed)
                {
                    if (rb.simulated) rb.simulated = false;
                }
                else if (state == Core.GameState.Playing)
                {
                    if (!rb.simulated) rb.simulated = true;
                }
            }
        }
    }
}
