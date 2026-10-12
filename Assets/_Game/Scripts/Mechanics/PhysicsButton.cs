using UnityEngine;
using PhysicsDraw.Core;

namespace PhysicsDraw.Mechanics
{
    public class PhysicsButton : MonoBehaviour, IResettable
    {
        [Tooltip("The objects that implement IActivatable to trigger.")]
        public GameObject[] targets;

        public bool oneShot = true;
        public float visualPressDepth = 0.2f;

        private bool isPressed = false;
        private Vector3 initialPos;
        private Transform buttonVisual;

        private void Awake()
        {
            // Assuming the button visual is the first child
            if (transform.childCount > 0)
                buttonVisual = transform.GetChild(0);
            else
                buttonVisual = transform;

            initialPos = buttonVisual.localPosition;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isPressed && oneShot) return;

            int layer = collision.gameObject.layer;
            if (layer == LayerMask.NameToLayer("Ball") || layer == LayerMask.NameToLayer("Drawing") || layer == LayerMask.NameToLayer("DynamicEnvironment"))
            {
                PressButton();
            }
        }

        private void PressButton()
        {
            if (isPressed && oneShot) return;

            isPressed = true;
            buttonVisual.localPosition = initialPos - new Vector3(0, visualPressDepth, 0);

            // Notify targets
            if (targets != null)
            {
                foreach (var t in targets)
                {
                    if (t != null)
                    {
                        var activatables = t.GetComponents<IActivatable>();
                        foreach (var act in activatables)
                        {
                            act.Activate();
                        }
                    }
                }
            }
        }

        public void ResetObject()
        {
            isPressed = false;
            buttonVisual.localPosition = initialPos;
        }
    }
}
