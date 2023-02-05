using System;
using UnityEngine;

namespace Infrastructure.Input.InputService
{
    public class MobileInputService : MonoBehaviour, IInputService, IUIInputController
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";
        private const string ThrowCurveAxisName = "ThrowCurve";

        public Vector2 InputDirection => new Vector2
        (
            SimpleInput.GetAxis(HorizontalAxisName),
            SimpleInput.GetAxis(VerticalAxisName)
        );

        public bool TouchHeldDown => SimpleInput.GetMouseButton(0);
        public bool ThrowButtonHeldDown { get; private set; }
        public bool PassButtonHeldDown { get; private set; }
        public bool DunkButtonHeldDown { get; private set; }
        public bool ChangePlayerButtonHeldDown { get; private set; }

        public Vector3 PointerPosition => UnityEngine.Input.mousePosition;
        public float ThrowCurve => SimpleInput.GetAxis(ThrowCurveAxisName);

        public event Action TouchDown;
        public event Action ThrowButtonDown;
        public event Action DunkButtonDown;
        public event Action ChangePlayerButtonDown;
        public event Action PassButtonDown;

        public event Action TouchUp;
        public event Action ThrowButtonUp;
        public event Action DunkButtonUp;
        public event Action ChangePlayerButtonUp;
        public event Action PassButtonUp;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (SimpleInput.GetMouseButtonDown(0))
                TouchDown?.Invoke();

            if (SimpleInput.GetMouseButtonUp(0))
                TouchUp?.Invoke();
        }

        public void OnUIThrowButtonDown()
        {
            ThrowButtonDown?.Invoke();
            ThrowButtonHeldDown = true;
        }

        public void OnUIPassButtonDown()
        {
            PassButtonDown?.Invoke();
            PassButtonHeldDown = true;
        }

        public void OnUIDunkButtonDown()
        {
            DunkButtonDown?.Invoke();
            DunkButtonHeldDown = true;
        }

        public void OnUIChangePlayerButtonDown()
        {
            ChangePlayerButtonDown?.Invoke();
            ChangePlayerButtonHeldDown = true;
        }

        public void OnUIThrowButtonUp()
        {
            ThrowButtonUp?.Invoke();
            ThrowButtonHeldDown = false;
        }

        public void OnUIPassButtonUp()
        {
            PassButtonUp?.Invoke();
            PassButtonHeldDown = false;
        }

        public void OnUIDunkButtonUp()
        {
            DunkButtonUp?.Invoke();
            DunkButtonHeldDown = false;
        }

        public void OnUIChangePlayerButtonUp()
        {
            ChangePlayerButtonUp?.Invoke();
            ChangePlayerButtonHeldDown = false;
        }
    }
}