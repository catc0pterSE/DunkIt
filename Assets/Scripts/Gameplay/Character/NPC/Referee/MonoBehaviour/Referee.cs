using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.NPC.Referee.MonoBehaviour
{
    using Ball.MonoBehavior;
    public class Referee: CharacterFacade
    {
        [SerializeField] private Animator _animator;
        public Animator Animator => _animator;

        public void Initialize(Ball ball) =>
            Ball = ball;
    }
}