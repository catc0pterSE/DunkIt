﻿using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class InputControlledDefenceState: IParameterlessState
    {
        private readonly MonoBehaviour.PlayerFacade _player;

        public InputControlledDefenceState(MonoBehaviour.PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.EnableInputControlledBrain();
            _player.EnablePlayerMover();
            _player.EnableDistanceTracker();
            _player.PrioritizeCamera();
            _player.FocusOnBallOwner();
        }
        
        public void Exit()
        {
            _player.DisablePlayerMover();
            _player.DisableInputControlledBrain();  
            _player.DisableDistanceTracker();
        }
    }
}