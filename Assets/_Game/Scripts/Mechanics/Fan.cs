using UnityEngine;
using PhysicsDraw.Core;

namespace PhysicsDraw.Mechanics
{
    [RequireComponent(typeof(AreaEffector2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Fan : MonoBehaviour, IActivatable, IResettable
    {
        public bool startActive = true;
        public float windForce = 15f;
        public float forceVariation = 2f;
        
        private AreaEffector2D effector;
        private bool isActive;
        private ParticleSystem particles;

        private void Awake()
        {
            effector = GetComponent<AreaEffector2D>();
            particles = GetComponentInChildren<ParticleSystem>();
            
            var col = GetComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.usedByEffector = true;
            
            // Assume the fan blows in the +Y direction of its local transform
            effector.forceAngle = 90f; // Relative to local transform if we rotate the object
            effector.useGlobalAngle = false;
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
                effector.forceMagnitude = windForce;
                effector.forceVariation = forceVariation;
            }

            if (particles != null)
            {
                if (isActive && !particles.isPlaying) particles.Play();
                else if (!isActive && particles.isPlaying) particles.Stop();
            }
        }
    }
}
