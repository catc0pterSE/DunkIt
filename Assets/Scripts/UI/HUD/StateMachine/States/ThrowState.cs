using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace UI.HUD.StateMachine.States
{
    public class ThrowState : IParameterState<PlayerFacade>
    {
        private readonly IGameplayHUD _gameplayHUD;

        public ThrowState(IGameplayHUD gameplayHUD)
        {
            _gameplayHUD = gameplayHUD;
        }

        public void Enter(PlayerFacade playerFacade)
        {
        }

        public void Exit()
        {
        }
    }
}