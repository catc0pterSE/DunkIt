using System;
using Infrastructure.ServiceManagement;
using UnityEngine;

namespace Infrastructure.Input.InputService
{
    public interface IInputService : IService
    {
        public Vector2 InputDirection { get; }
        public bool Clicked { get; }

        public Vector3 PointerPosition { get; }

        public event Action ThrowButtonPressed;

        public event Action PassButtonPressed;

        public event Action DunkButtonPressed;
        
        public event Action ChangePlayerButtonPressed;
    }
}