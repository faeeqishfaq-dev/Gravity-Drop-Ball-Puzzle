using UnityEngine;
using PhysicsDraw.Core;

namespace PhysicsDraw.Mechanics
{
    [RequireComponent(typeof(SurfaceEffector2D))]
    public class Conveyor : MonoBehaviour, IActivatable, IResettable
    {
        public bool startActive = true;
        public float speed = 5f;

        private SurfaceEffector2D effector;
        private bool isActive;

        private void Awake()
        {
            effector = GetComponent<SurfaceEffector2D>();
        }

        private void Start()
        {
            isActive = startActive;
            ApplyState();
        }

        public void Activate()
        {
            isActive = true;
            ApplyState();
        }

        public void Deactivate()
        {
            isActive = false;
            ApplyState();
        }

        public void ResetObject()
        {
            isActive = startActive;
            ApplyState();
        }

        private void ApplyState()
        {
            if (effector != null)
            {
                effector.enabled = isActive;
                effector.speed = speed;
            }
        }
    }
}
