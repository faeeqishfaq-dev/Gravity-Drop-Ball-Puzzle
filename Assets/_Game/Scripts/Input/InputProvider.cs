using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace PhysicsDraw.InputSystem
{
    public class InputProvider : MonoBehaviour
    {
        public static InputProvider Instance { get; private set; }

        public event Action<Vector2> OnPointerDown;
        public event Action<Vector2> OnPointerMove;
        public event Action<Vector2> OnPointerUp;

        private bool isPointerDown = false;

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
                return;
            }

            EnhancedTouchSupport.Enable();
        }

        private void Update()
        {
            var state = PhysicsDraw.Core.GameManager.Instance != null ? PhysicsDraw.Core.GameManager.Instance.CurrentState : PhysicsDraw.Core.GameState.Playing;
            if (state != PhysicsDraw.Core.GameState.Playing && state != PhysicsDraw.Core.GameState.Ready)
            {
                if (isPointerDown)
                {
                    isPointerDown = false;
                    OnPointerUp?.Invoke(GetPointerPosition());
                }
                return;
            }

            HandleUnifiedInput();
        }

        private void HandleUnifiedInput()
        {
            var pointer = Pointer.current;
            if (pointer == null) return;

            Vector2 pos = pointer.position.ReadValue();

            if (pointer.press.wasPressedThisFrame)
            {
                isPointerDown = true;
                OnPointerDown?.Invoke(pos);
            }
            else if (pointer.press.isPressed && isPointerDown)
            {
                OnPointerMove?.Invoke(pos);
            }
            else if (pointer.press.wasReleasedThisFrame && isPointerDown)
            {
                isPointerDown = false;
                OnPointerUp?.Invoke(pos);
            }
        }

        private Vector2 GetPointerPosition()
        {
            if (Pointer.current != null) return Pointer.current.position.ReadValue();
            return Vector2.zero;
        }

        public void SimulatePointerDown(Vector2 pos)
        {
            isPointerDown = true;
            OnPointerDown?.Invoke(pos);
        }

        public void SimulatePointerMove(Vector2 pos)
        {
            if (isPointerDown) OnPointerMove?.Invoke(pos);
        }

        public void SimulatePointerUp(Vector2 pos)
        {
            isPointerDown = false;
            OnPointerUp?.Invoke(pos);
        }
    }
}
