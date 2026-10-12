using UnityEngine;

namespace PhysicsDraw.Mechanics
{
    public class Spring : MonoBehaviour
    {
        public Vector2 launchDirection = Vector2.up;
        public float launchForce = 15f;
        public float cooldown = 0.5f;

        private float lastTriggerTime = -99f;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (Time.time < lastTriggerTime + cooldown) return;

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Reset velocity in the launch direction to ensure consistent bounce
                Vector2 currentVel = rb.velocity;
                Vector2 dirNorm = launchDirection.normalized;
                
                // Remove velocity component against the spring
                float dot = Vector2.Dot(currentVel, dirNorm);
                if (dot < 0)
                {
                    currentVel -= dirNorm * dot;
                }
                
                rb.velocity = currentVel;
                rb.AddForce(dirNorm * launchForce, ForceMode2D.Impulse);

                lastTriggerTime = Time.time;
                
                // Optional: Play spring animation/VFX here
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, launchDirection.normalized * 2f);
        }
    }
}
