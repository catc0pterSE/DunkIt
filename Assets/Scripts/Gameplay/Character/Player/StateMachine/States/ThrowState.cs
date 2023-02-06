﻿
using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using UnityEngine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class ThrowState : IParameterState<Vector3>
    {
        private readonly MonoBehaviour.PlayerFacade _player;

        public ThrowState(MonoBehaviour.PlayerFacade player)
        {
            _player = player;
        }

        public void Enter(Vector3 ringPosition)
        {
            _player.DisableInputControlledBrain();
            _player.DisableAIControlledBrain();
            _player.RotateTo(ringPosition, StartThrow);
        }

        private void StartThrow()
        {
            _player.DisablePlayerMover();
            _player.EnableBallThrower();
            SubscribeOnBallThrown();
        }

        public void Exit()
        {
            _player.DisableBallThrower();
            UnsubscribeFromBallThrown();
        }

        private void SubscribeOnBallThrown() =>
            _player.BallThrown += OnBallThrown;
        
        private void UnsubscribeFromBallThrown() =>
            _player.BallThrown -= OnBallThrown;

        private void OnBallThrown()
        {
            _player.DisableBallThrower();
            _player.PrioritizeCamera();
            _player.FocusOnBall();
        }
    }
}