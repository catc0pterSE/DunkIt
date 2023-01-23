using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character
{
    using Ball.MonoBehavior;

    public abstract class Character : SwitchableMonoBehaviour
    {
        [SerializeField] private Transform _ballPosition;

        private bool _ownsBall;

        public Transform BallPosition => _ballPosition;

        public void TakeBall() =>
            _ownsBall = true;

        public void LoseBall() =>
            _ownsBall = false;
    }
}