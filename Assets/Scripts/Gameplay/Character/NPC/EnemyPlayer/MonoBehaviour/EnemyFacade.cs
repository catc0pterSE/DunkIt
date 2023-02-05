using System;
using Gameplay.Character.NPC.EnemyPlayer.StateMachine;
using UnityEngine;

namespace Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour
{
    using Ball.MonoBehavior;
    public class EnemyFacade : BasketballPlayerFacade
    {
        [SerializeField] private Animator _animator;

        private EnemyStateMachine _stateMachine;
        
        public event Action BallThrown;
        
        public EnemyStateMachine StateMachine => _stateMachine ??= new EnemyStateMachine(this);
        public Animator Animator => _animator;
        
    }
}