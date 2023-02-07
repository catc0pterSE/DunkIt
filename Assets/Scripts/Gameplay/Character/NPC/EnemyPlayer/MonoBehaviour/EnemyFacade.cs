using System;
using Gameplay.Character.NPC.EnemyPlayer.StateMachine;
using Gameplay.Character.NPC.EnemyPlayer.StateMachine.States;
using UnityEngine;

namespace Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour
{
    using Ball.MonoBehavior;
    public class EnemyFacade : BasketballPlayerFacade
    {
        [SerializeField] private Animator _animator;

        private EnemyStateMachine _stateMachine;
        public Animator Animator => _animator;
        
        public event Action BallThrown;
        
        private EnemyStateMachine StateMachine => _stateMachine ??= new EnemyStateMachine(this);
        
        public void EnterNotControlledState() =>
            StateMachine.Enter<NotControlledState>();
        
        public void EnterAIControlledState() =>
            StateMachine.Enter<AIControlledState>();
        
        public void EnterIdleState() =>
            StateMachine.Enter<IdleState>();
        
        public void EnterContestingBallState() =>
            StateMachine.Enter<ContestingBallState>();

    }
}