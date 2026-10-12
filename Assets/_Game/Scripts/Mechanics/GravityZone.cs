using System.Collections.Generic;
using UnityEngine;

namespace PhysicsDraw.Mechanics
{
    public class GravityZone : MonoBehaviour
    {
        public Vector2 localGravity = new Vector2(0, 9.81f);
        public bool overrideGlobalGravity = true;

        private Dictionary<Rigidbody2D, float> originalGravityScales = new Dictionary<Rigidbody2D, float>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (overrideGlobalGravity)
                {
                    if (!originalGravityScales.ContainsKey(rb))
                    {
                        originalGravityScales.Add(rb, rb.gravityScale);
                        rb.gravityScale = 0f;
                    }
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Apply local gravity manually
                rb.AddForce(localGravity * rb.mass, ForceMode2D.Force);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                if (originalGravityScales.ContainsKey(rb))
                {
                    rb.gravityScale = originalGravityScales[rb];
                    originalGravityScales.Remove(rb);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 1f, 1f, 0.5f);
            var col = GetComponent<Collider2D>();
            if (col != null)
            {
                Gizmos.DrawCube(col.bounds.center, col.bounds.size);
            }
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, localGravity.normalized * 2f);
        }
    }
}
