using System;
using UnityEngine;

namespace Infrastructure.Input.InputService
{
    public class MobileInputService : IInputService, IUIInputController
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        public Vector2 InputDirection => new Vector2
        (
            SimpleInput.GetAxis(Horizontal),
            SimpleInput.GetAxis(Vertical)
        );

        public bool Clicked => SimpleInput.GetMouseButtonDown(0);
        public Vector3 PointerPosition => UnityEngine.Input.mousePosition;

        public event Action ThrowButtonPressed;
        public event Action PassButtonPressed;
        public event Action DunkButtonPressed;

        public void OnUIThrowButtonClicked()
        {
            ThrowButtonPressed?.Invoke();
        }

        public void OnUIPassButtonClicked()
        {
            PassButtonPressed?.Invoke();
        }

        public void OnUIDunkButtonClicked()
        {
            DunkButtonPressed?.Invoke();
        }
    }
}