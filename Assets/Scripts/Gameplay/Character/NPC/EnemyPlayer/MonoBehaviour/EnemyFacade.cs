using Gameplay.Character.NPC.EnemyPlayer.StateMachine;
using UnityEngine;

namespace Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour
{
    using Ball.MonoBehavior;
    public class EnemyFacade : Character
    {
        [SerializeField] private Animator _animator;

        private EnemyStateMachine _stateMachine;

        public EnemyStateMachine StateMachine => _stateMachine ??= new EnemyStateMachine(this);
        public Animator Animator => _animator;
    }
}