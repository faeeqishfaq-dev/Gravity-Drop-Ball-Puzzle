using System.Collections.Generic;
using UnityEngine;

namespace PhysicsDraw.Drawing
{
    [RequireComponent(typeof(LineRenderer))]
    public class DrawnPhysicsObject : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private Rigidbody2D rb;
        
        private List<GameObject> colliderSegments = new List<GameObject>();
        private float currentWidth;
        private PhysicsMaterial2D currentMaterial;

        public void InitializePreview(List<Vector2> points, float width, PhysicsMaterial2D mat)
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;

            currentWidth = width;
            currentMaterial = mat;

            // Make sure the collider exists but might not have valid points until length >= 2
            UpdatePreview(points);
        }

        public void UpdatePreview(List<Vector2> points)
        {
            if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
            
            lineRenderer.positionCount = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                // In world space, set position directly
                lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, 0f));
            }

            if (points.Count >= 2)
            {
                UpdateCapsuleColliders(points);
            }
        }

        private void UpdateCapsuleColliders(List<Vector2> points)
        {
            int requiredSegments = points.Count - 1;
            
            while (colliderSegments.Count < requiredSegments)
            {
                GameObject segObj = new GameObject("Segment_" + colliderSegments.Count);
                segObj.transform.SetParent(this.transform);
                segObj.transform.localPosition = Vector3.zero;
                segObj.layer = this.gameObject.layer;
                
                var cap = segObj.AddComponent<CapsuleCollider2D>();
                cap.direction = CapsuleDirection2D.Horizontal;
                cap.sharedMaterial = currentMaterial;
                cap.isTrigger = false;
                
                colliderSegments.Add(segObj);
            }
            
            for (int i = requiredSegments; i < colliderSegments.Count; i++)
            {
                colliderSegments[i].SetActive(false);
            }

            for (int i = 0; i < requiredSegments; i++)
            {
                var segObj = colliderSegments[i];
                segObj.SetActive(true);
                
                Vector2 p1 = points[i];
                Vector2 p2 = points[i + 1];
                
                float dist = Vector2.Distance(p1, p2);
                Vector2 dir = p2 - p1;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                
                // Position colliders based on world points, but local relative to parent which is at (0,0) initially
                segObj.transform.position = new Vector3((p1.x + p2.x) / 2f, (p1.y + p2.y) / 2f, 0f);
                segObj.transform.rotation = Quaternion.Euler(0, 0, angle);
                
                var cap = segObj.GetComponent<CapsuleCollider2D>();
                // Horizontal capsule: size.x is length, size.y is thickness
                cap.size = new Vector2(dist + currentWidth, currentWidth);
            }
        }

        public void FinalizePhysics(List<Vector2> simplifiedPoints, float massMultiplier, float minMass, float maxMass, float gravityScale)
        {
            // Set final rendered points and colliders
            UpdatePreview(simplifiedPoints);
            
            // Calculate Mass
            float length = CalculateLength(simplifiedPoints);
            float calculatedMass = Mathf.Clamp(length * massMultiplier, minMass, maxMass);

            // Convert LineRenderer points to Local Space since useWorldSpace = false
            // The object's transform is currently at (0,0,0) but let's be mathematically rigorous
            for (int i = 0; i < simplifiedPoints.Count; i++)
            {
                Vector3 localPos = transform.InverseTransformPoint(new Vector3(simplifiedPoints[i].x, simplifiedPoints[i].y, 0f));
                lineRenderer.SetPosition(i, localPos);
            }
            
            // Convert Colliders to Local Space
            for (int i = 0; i < simplifiedPoints.Count - 1; i++)
            {
                var segObj = colliderSegments[i];
                Vector2 p1 = simplifiedPoints[i];
                Vector2 p2 = simplifiedPoints[i + 1];
                float angle = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * Mathf.Rad2Deg;
                
                segObj.transform.localPosition = transform.InverseTransformPoint(new Vector3((p1.x + p2.x) / 2f, (p1.y + p2.y) / 2f, 0f));
                segObj.transform.localRotation = Quaternion.Euler(0, 0, angle - transform.eulerAngles.z);
            }

            // Add Rigidbody2D
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.mass = calculatedMass;
            rb.gravityScale = gravityScale;
        }

        private float CalculateLength(List<Vector2> points)
        {
            float length = 0f;
            for (int i = 1; i < points.Count; i++)
            {
                length += Vector2.Distance(points[i - 1], points[i]);
            }
            return length;
        }
    }
}
