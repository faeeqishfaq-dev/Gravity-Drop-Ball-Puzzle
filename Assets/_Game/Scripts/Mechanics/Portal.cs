using System.Collections;
using UnityEngine;

namespace PhysicsDraw.Mechanics
{
    public class Portal : MonoBehaviour
    {
        public Portal pairedPortal;
        public Transform exitPoint;
        
        [Tooltip("If true, exit velocity rotates relative to the portal orientation difference.")]
        public bool rotateVelocity = true;

        [Tooltip("Cooldown to prevent instant re-entry loops.")]
        public float cooldownTime = 0.5f;

        private float lastUseTime = -99f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (pairedPortal == null || exitPoint == null) return;
            
            if (Time.time < lastUseTime + cooldownTime) return;

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Notify paired portal so it doesn't immediately send the object back
                pairedPortal.lastUseTime = Time.time;
                this.lastUseTime = Time.time;

                TeleportObject(collision.transform, rb);
            }
        }

        private void TeleportObject(Transform obj, Rigidbody2D rb)
        {
            // Position
            obj.position = pairedPortal.exitPoint.position;

            if (rotateVelocity)
            {
                // Calculate difference in rotation between this portal's entry direction and paired portal's exit direction
                float rotationDiff = pairedPortal.exitPoint.eulerAngles.z - this.transform.eulerAngles.z;
                
                // Usually portals face opposite ways, so entering 'in' means coming 'out' the other side.
                // Assuming +Y is 'out' for both, we rotate by 180 as well.
                rotationDiff += 180f;

                Quaternion rotation = Quaternion.Euler(0, 0, rotationDiff);
                Vector2 currentVel = rb.velocity;
                Vector2 newVel = rotation * currentVel;
                
                rb.velocity = newVel;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.5f, 0, 0.8f, 0.5f);
            if (exitPoint != null)
            {
                Gizmos.DrawRay(exitPoint.position, exitPoint.up * 2f);
            }
            if (pairedPortal != null)
            {
                Gizmos.DrawLine(transform.position, pairedPortal.transform.position);
            }
        }
    }
}
