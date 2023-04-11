using System;
using Infrastructure.ServiceManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Infrastructure.Input.InputService
{
    public interface IInputService : IService
    {
        public Vector2 MovementInput { get; }
        public Vector2 PointerMovement { get; }
        public Vector3 PointerPosition { get; }
        public bool PointerHeldDown { get; }
        public bool ThrowButtonHeldDown { get;  }
        public bool PassButtonHeldDown { get;  }
        public bool DunkButtonHeldDown { get;  }
        public bool ChangePlayerButtonHeldDown { get; }
        
        public event Action PointerDown;
        public event Action PointerUp;
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