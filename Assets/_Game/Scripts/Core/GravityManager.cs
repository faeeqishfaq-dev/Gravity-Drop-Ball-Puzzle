using UnityEngine;

namespace PhysicsDraw.Core
{
    public class GravityManager : MonoBehaviour
    {
        public static GravityManager Instance { get; private set; }

        private Vector2 defaultGravity = new Vector2(0, -9.81f);

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetDefaultGravity(Vector2 newDefault)
        {
            defaultGravity = newDefault;
            ResetGravity();
        }

        public void ChangeGravity(Vector2 newGravity)
        {
            Physics2D.gravity = newGravity;
        }

        public void ResetGravity()
        {
            Physics2D.gravity = defaultGravity;
        }
    }
}
