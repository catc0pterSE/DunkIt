﻿using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class InputControlledAttackState : IParameterlessState
    {
        private readonly MonoBehaviour.PlayerFacade _player;

        public InputControlledAttackState(MonoBehaviour.PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.EnableCharacterController();
            _player.EnableInputControlledAttackBrain();
            _player.EnablePlayerMover();
            _player.PrioritizeCamera();
            _player.FocusOnOppositeBasket();
            _player.EnableDistanceTracker();
            _player.EnableFightForBallTriggerZone();
        }
        
        public void Exit()
        {
            _player.DisableCharacterController();
            _player.DisablePlayerMover();
            _player.DisableInputControlledAttackBrain();
            _player.DisableDistanceTracker();
            _player.DisableFightForBallTriggerZone();
        }
    }
}