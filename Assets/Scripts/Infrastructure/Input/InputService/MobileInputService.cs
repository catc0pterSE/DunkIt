using System;
using UnityEngine;

namespace Infrastructure.Input.InputService
{
    public class MobileInputService : IInputService, IUIInputController
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";
        private const string ThrowCurveAxisName = "ThrowCurve";

        public Vector2 InputDirection => new Vector2
        (
            SimpleInput.GetAxis(HorizontalAxisName),
            SimpleInput.GetAxis(VerticalAxisName)
        );

        public bool Clicked => SimpleInput.GetMouseButtonDown(0);
        public Vector3 PointerPosition => UnityEngine.Input.mousePosition;

        public float ThrowCurve => SimpleInput.GetAxis(ThrowCurveAxisName);

        public event Action ThrowButtonPressed;
        public event Action PassButtonPressed;
        public event Action DunkButtonPressed;
        public event Action ChangePlayerButtonPressed;

        public void OnUIThrowButtonClicked() =>
            ThrowButtonPressed?.Invoke();

        public void OnUIPassButtonClicked() =>
            PassButtonPressed?.Invoke();

        public void OnUIDunkButtonClicked() =>
            DunkButtonPressed?.Invoke();

        public void OnUIChangePlayerButtonClicked() =>
            ChangePlayerButtonPressed?.Invoke();
    }
}