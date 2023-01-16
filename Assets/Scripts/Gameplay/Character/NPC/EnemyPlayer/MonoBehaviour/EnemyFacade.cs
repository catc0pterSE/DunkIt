﻿using Gameplay.Character.NPC.EnemyPlayer.StateMachine;
using UnityEngine;

namespace Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour
{
    public class EnemyFacade : UnityEngine.MonoBehaviour
    {
        [SerializeField] private Attack _attack;
        [SerializeField] private Defence _defence;
        [SerializeField] private Animator _animator;

        private EnemyStateMachine _stateMachine;

        public EnemyStateMachine StateMachine => _stateMachine ??= new EnemyStateMachine(this);
        public Animator Animator => _animator;
        
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