﻿using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class ThrowState : IParameterlessState
    {
        private readonly PlayerFacade _player;

        public ThrowState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.DisablePlayerMover();
            _player.DisableInputControlledBrain();
            _player.DisableAIControlledBrain();
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