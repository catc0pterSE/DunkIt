using System;
using Infrastructure.ServiceManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Infrastructure.Input.InputService
{
    public interface IInputService : IService
    {
        public Vector2 InputDirection { get; }
        public Vector3 PointerPosition { get; }
        public float ThrowCurve { get; }
      
        public bool TouchHeldDown { get; }
        public bool ThrowButtonHeldDown { get;  }
        public bool PassButtonHeldDown { get;  }
        public bool DunkButtonHeldDown { get;  }
        public bool ChangePlayerButtonHeldDown { get; }
        
        public event Action TouchDown;
        public event Action TouchUp;
        public event Action ThrowButtonDown;
        public event Action DunkButtonDown;
        public event Action ChangePlayerButtonDown;
        public event Action PassButtonDown;
        public event Action ThrowButtonUp;
        public event Action DunkButtonUp;
        public event Action ChangePlayerButtonUp;
        public event Action PassButtonUp;
    }
}