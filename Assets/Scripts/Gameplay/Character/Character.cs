using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character
{
    using Ball.MonoBehavior;

    public abstract class Character : SwitchableMonoBehaviour
    {
        [SerializeField] private Transform _ballPosition;

        public bool OwnsBall { get; private set; }

        public Transform BallPosition => _ballPosition;

        public virtual void TakeBall() =>
            OwnsBall = true;

        public virtual void LoseBall() =>
            OwnsBall = false;
    }
}