using UnityEngine;
using PhysicsDraw.Core;

namespace PhysicsDraw.Mechanics
{
    public class GravitySwitch : MonoBehaviour, IActivatable
    {
        public Vector2 newGravityDirection = Vector2.up;
        public float strength = 9.81f;
        public bool revertOnDeactivate = true;

        public void Activate()
        {
            if (GravityManager.Instance != null)
            {
                GravityManager.Instance.ChangeGravity(newGravityDirection.normalized * strength);
            }
        }

        public void Deactivate()
        {
            if (revertOnDeactivate && GravityManager.Instance != null)
            {
                GravityManager.Instance.ResetGravity();
            }
        }
    }
}
