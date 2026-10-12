using UnityEngine;
using PhysicsDraw.Core;
using System.Collections.Generic;

namespace PhysicsDraw.Mechanics
{
    public class PressurePlate : MonoBehaviour, IResettable
    {
        public GameObject[] targets;
        public float visualPressDepth = 0.2f;

        private Vector3 initialPos;
        private Transform plateVisual;
        private HashSet<Collider2D> objectsOnPlate = new HashSet<Collider2D>();

        private void Awake()
        {
            if (transform.childCount > 0)
                plateVisual = transform.GetChild(0);
            else
                plateVisual = transform;

            initialPos = plateVisual.localPosition;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            int layer = collision.gameObject.layer;
            if (layer == LayerMask.NameToLayer("Ball") || layer == LayerMask.NameToLayer("Drawing") || layer == LayerMask.NameToLayer("DynamicEnvironment"))
            {
                if (objectsOnPlate.Count == 0)
                {
                    ActivateTargets(true);
                }
                objectsOnPlate.Add(collision);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (objectsOnPlate.Contains(collision))
            {
                objectsOnPlate.Remove(collision);
                if (objectsOnPlate.Count == 0)
                {
                    ActivateTargets(false);
                }
            }
        }

        private void ActivateTargets(bool activate)
        {
            plateVisual.localPosition = activate ? initialPos - new Vector3(0, visualPressDepth, 0) : initialPos;

            if (targets != null)
            {
                foreach (var t in targets)
                {
                    if (t != null)
                    {
                        var activatables = t.GetComponents<IActivatable>();
                        foreach (var act in activatables)
                        {
                            if (activate) act.Activate();
                            else act.Deactivate();
                        }
                    }
                }
            }
        }

        public void ResetObject()
        {
            objectsOnPlate.Clear();
            ActivateTargets(false);
        }
    }
}
