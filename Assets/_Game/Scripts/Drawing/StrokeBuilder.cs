using System.Collections.Generic;
using UnityEngine;

namespace PhysicsDraw.Drawing
{
    public class StrokeBuilder
    {
        public List<Vector2> Points { get; private set; } = new List<Vector2>();
        private float minDistanceSqr;

        public StrokeBuilder(float minPointDistance)
        {
            minDistanceSqr = minPointDistance * minPointDistance;
        }

        public void StartStroke(Vector2 startPoint)
        {
            Points.Clear();
            Points.Add(startPoint);
        }

        public bool AddPoint(Vector2 newPoint)
        {
            if (Points.Count == 0)
            {
                Points.Add(newPoint);
                return true;
            }

            Vector2 lastPoint = Points[Points.Count - 1];
            if (Vector2.SqrMagnitude(newPoint - lastPoint) >= minDistanceSqr)
            {
                Points.Add(newPoint);
                return true;
            }

            return false;
        }

        public List<Vector2> SimplifyStroke(float tolerance)
        {
            if (Points.Count < 3) return new List<Vector2>(Points);
            
            // Basic Ramer-Douglas-Peucker simplification
            return DouglasPeucker(Points, 0, Points.Count - 1, tolerance);
        }

        private List<Vector2> DouglasPeucker(List<Vector2> points, int startIndex, int endIndex, float tolerance)
        {
            float dmax = 0f;
            int index = startIndex;

            for (int i = startIndex + 1; i < endIndex; i++)
            {
                float d = PerpendicularDistance(points[i], points[startIndex], points[endIndex]);
                if (d > dmax)
                {
                    index = i;
                    dmax = d;
                }
            }

            List<Vector2> result = new List<Vector2>();

            if (dmax > tolerance)
            {
                var recResults1 = DouglasPeucker(points, startIndex, index, tolerance);
                var recResults2 = DouglasPeucker(points, index, endIndex, tolerance);

                result.AddRange(recResults1);
                result.RemoveAt(result.Count - 1);
                result.AddRange(recResults2);
            }
            else
            {
                result.Add(points[startIndex]);
                result.Add(points[endIndex]);
            }

            return result;
        }

        private float PerpendicularDistance(Vector2 pt, Vector2 lineStart, Vector2 lineEnd)
        {
            float dx = lineEnd.x - lineStart.x;
            float dy = lineEnd.y - lineStart.y;

            float mag = Mathf.Sqrt(dx * dx + dy * dy);
            if (mag > 0.0f)
            {
                dx /= mag;
                dy /= mag;
            }

            float pvx = pt.x - lineStart.x;
            float pvy = pt.y - lineStart.y;

            float pvdot = dx * pvx + dy * pvy;
            float ax = pvx - pvdot * dx;
            float ay = pvy - pvdot * dy;

            return Mathf.Sqrt(ax * ax + ay * ay);
        }
        
        public float CalculateLength(List<Vector2> points)
        {
            float length = 0f;
            for (int i = 1; i < points.Count; i++)
            {
                length += Vector2.Distance(points[i-1], points[i]);
            }
            return length;
        }
    }
}
