using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using UnityEngine;

namespace UI.HUD.StateMachine.States
{
    public class BallChasingState : IParameterState<PlayerFacade>
    {
        private readonly IGameplayHUD _gameplayHUD;

        public BallChasingState(IGameplayHUD gameplayHUD)
        {
            _gameplayHUD = gameplayHUD;
        }

        public void Enter(PlayerFacade playerFacade)
        {
            _gameplayHUD.SetChangePlayerAvailability(true);
        }

        public void Exit()
        {
            _gameplayHUD.SetChangePlayerAvailability(false);
        }
    }
}