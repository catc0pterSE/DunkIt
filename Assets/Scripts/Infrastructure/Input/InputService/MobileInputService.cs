using System;
using UnityEngine;

namespace Infrastructure.Input.InputService
{
    public class MobileInputService : MonoBehaviour, IInputService, IUIInputController
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";
        private const string MouseXAxisName = "Mouse X";
        private const string MouseYAxisName = "Mouse Y";

       public Vector2 PointerMovement => new Vector2
        (
            SimpleInput.GetAxis(MouseXAxisName),
            SimpleInput.GetAxis(MouseYAxisName)
        );

        public bool PointerHeldDown => SimpleInput.GetMouseButton(0);
        public bool ThrowButtonHeldDown { get; private set; }
        public bool PassButtonHeldDown { get; private set; }
        public bool DunkButtonHeldDown { get; private set; }
        public bool ChangePlayerButtonHeldDown { get; private set; }
        public bool JumpButtonHeldDown { get; private set; }
        
        public Vector3 PointerPosition => UnityEngine.Input.mousePosition;

        public event Action<Vector2> MovementInputReceived;
        public event Action PointerDown;
        public event Action ThrowButtonDown;
        public event Action DunkButtonDown;
        public event Action ChangePlayerButtonDown;
        public event Action PassButtonDown;
        public event Action JumpButtonDown;

        public event Action PointerUp;
        public event Action ThrowButtonUp;
        public event Action DunkButtonUp;
        public event Action ChangePlayerButtonUp;
        public event Action PassButtonUp;
        public event Action JumpButtonUp;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            float xMovement = SimpleInput.GetAxis(HorizontalAxisName);
            float zMovement = SimpleInput.GetAxis(VerticalAxisName);
            
            if (Mathf.Abs(xMovement) > Mathf.Epsilon || Mathf.Abs(zMovement) > Mathf.Epsilon )
                MovementInputReceived?.Invoke(new Vector2
                (
                    SimpleInput.GetAxis(HorizontalAxisName),
                    SimpleInput.GetAxis(VerticalAxisName)
                ));

            if (SimpleInput.GetMouseButtonDown(0))
                PointerDown?.Invoke();


            if (SimpleInput.GetMouseButtonUp(0))
                PointerUp?.Invoke();
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

        public void OnUIJumpButtonUP()
        {
            JumpButtonUp?.Invoke();
            JumpButtonHeldDown = false;
        }

        public void OnUIJumpButtonDown()
        {
            JumpButtonDown?.Invoke();
            JumpButtonHeldDown = true;
        }
    }
}