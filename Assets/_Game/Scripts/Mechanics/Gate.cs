using UnityEngine;
using PhysicsDraw.Core;

namespace PhysicsDraw.Mechanics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Gate : MonoBehaviour, IResettable, IActivatable
    {
        public Vector3 closedOffset = Vector3.zero;
        public Vector3 openOffset = new Vector3(0, 2f, 0);
        public float speed = 3f;
        public bool startOpen = false;

        private Rigidbody2D rb;
        private Vector3 initialPos;
        private Vector3 targetPos;
        private bool isOpen = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            initialPos = transform.position;
        }

        private void Start()
        {
            isOpen = startOpen;
            targetPos = isOpen ? initialPos + openOffset : initialPos + closedOffset;
            rb.position = targetPos;
        }

        private void FixedUpdate()
        {
            Vector3 currentPos = rb.position;
            if (Vector3.Distance(currentPos, targetPos) > 0.01f)
            {
                Vector3 newPos = Vector3.MoveTowards(currentPos, targetPos, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
        }

        public void Activate()
        {
            isOpen = true;
            targetPos = initialPos + openOffset;
        }

        public void Deactivate()
        {
            isOpen = false;
            targetPos = initialPos + closedOffset;
        }

        public void ResetObject()
        {
            isOpen = startOpen;
            targetPos = isOpen ? initialPos + openOffset : initialPos + closedOffset;
            rb.position = targetPos;
            transform.position = targetPos;
        }
        
        private void OnDrawGizmosSelected()
        {
            Vector3 closedPos = Application.isPlaying ? initialPos + closedOffset : transform.position + closedOffset;
            Vector3 openPos = Application.isPlaying ? initialPos + openOffset : transform.position + openOffset;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(closedPos, 0.15f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(openPos, 0.15f);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(closedPos, openPos);
        }
    }
}
