﻿using System;
using Infrastructure.ServiceManagement;
using UnityEngine;

namespace Infrastructure.Input
{
    public interface IInputService : IService
    {
        public Vector2 InputDirection { get; }
        public bool Clicked { get; }
    }
}