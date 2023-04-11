using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character
{
    using Ball.MonoBehavior;

    public abstract class CharacterFacade : SwitchableMonoBehaviour
    {
        [SerializeField] private Transform _ballPosition;

        protected Ball Ball { get; set; }

        public bool OwnsBall => Ball.Owner == this;

        public Transform BallPosition => _ballPosition;
    }
}