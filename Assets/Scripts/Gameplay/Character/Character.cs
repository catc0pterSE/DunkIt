using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character
{
    using Ball.MonoBehavior;

    public abstract class Character : SwitchableMonoBehaviour
    {
        [SerializeField] private Transform _ballPosition;

        protected Ball Ball;

        public bool OwnsBall => Ball.Owner == this;

        public Transform BallPosition => _ballPosition;
    }
}