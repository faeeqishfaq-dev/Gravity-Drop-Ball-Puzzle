using UnityEngine;
using PhysicsDraw.Core;

namespace PhysicsDraw.Mechanics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovingPlatform : MonoBehaviour, IResettable, IActivatable
    {
        public Vector3 pointA;
        public Vector3 pointB;
        public float speed = 2f;
        public float waitTime = 1f;
        public bool startActive = true;
        
        public enum LoopType { PingPong, Loop, Once }
        public LoopType loopType = LoopType.PingPong;

        private Rigidbody2D rb;
        private Vector3 targetPos;
        private float waitTimer = 0f;
        private bool isActive = false;
        private bool isMovingToB = true;

        private Vector3 initialPos;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            initialPos = transform.position;
            // Point A and B are usually relative to the starting position for easier design
            pointA += initialPos;
            pointB += initialPos;
        }

        private void Start()
        {
            isActive = startActive;
            targetPos = pointB;
        }

        private void FixedUpdate()
        {
            if (!isActive) return;

            if (waitTimer > 0)
            {
                waitTimer -= Time.fixedDeltaTime;
                return;
            }

            Vector3 currentPos = rb.position;
            Vector3 newPos = Vector3.MoveTowards(currentPos, targetPos, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (Vector3.Distance(newPos, targetPos) < 0.01f)
            {
                ReachTarget();
            }
        }

        private void ReachTarget()
        {
            waitTimer = waitTime;

            if (loopType == LoopType.PingPong)
            {
                isMovingToB = !isMovingToB;
                targetPos = isMovingToB ? pointB : pointA;
            }
            else if (loopType == LoopType.Loop)
            {
                // Snap back to start and continue
                rb.position = pointA;
                targetPos = pointB;
            }
            else if (loopType == LoopType.Once)
            {
                isActive = false;
            }
        }

        public void Activate()
        {
            isActive = true;
        }

        public void Deactivate()
        {
            isActive = false;
        }

        public void ResetObject()
        {
            isActive = startActive;
            isMovingToB = true;
            targetPos = pointB;
            waitTimer = 0f;
            transform.position = initialPos;
            rb.position = initialPos;
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 a = Application.isPlaying ? pointA : transform.position + pointA;
            Vector3 b = Application.isPlaying ? pointB : transform.position + pointB;
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(a, 0.2f);
            Gizmos.DrawSphere(b, 0.2f);
            Gizmos.DrawLine(a, b);
        }
    }
}
