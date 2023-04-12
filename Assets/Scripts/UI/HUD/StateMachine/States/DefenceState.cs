﻿using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace UI.HUD.StateMachine.States
{
    public class DefenceState : IParameterState<PlayerFacade>
    {
        private readonly IGameplayHUD _gameplayHUD;

        public DefenceState(IGameplayHUD gameplayHUD)
        {
            _gameplayHUD = gameplayHUD;
        }

        public void Enter(PlayerFacade playerFacade)
        {
            _gameplayHUD.SetChangePlayerAvailability(true);
            _gameplayHUD.SetMovementAvailability(true);
        }

        public void Exit()
        {
            _gameplayHUD.SetChangePlayerAvailability(false);
            _gameplayHUD.SetMovementAvailability(false);
        }
    }
}