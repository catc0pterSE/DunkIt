using System;
using Gameplay.NPC.EnemyPlayer.StateMachine;
using UnityEngine;

namespace Gameplay.NPC.EnemyPlayer.MonoBehaviour
{
    public class EnemyFacade : UnityEngine.MonoBehaviour
    {
        [SerializeField] private Attack _attack;
        [SerializeField] private Defence _defence;

        private EnemyStateMachine _stateMachine;

        public EnemyStateMachine StateMachine => _stateMachine ??= new EnemyStateMachine(this);
        
        public void EnableAttack() =>
            _attack.Enable();

        public void DisableAttack() =>
            _attack.Disable();

        public void EnableDefence() =>
            _defence.Enable();

        public void DisableDefence() =>
            _defence.Disable();
    }
}