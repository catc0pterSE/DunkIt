using Modules.StateMachine;

namespace UI.HUD.StateMachine.States
{
    public class DefenceState: IParameterlessState
    {
        private readonly IGameplayHUD _gameplayHUD;

        public DefenceState(IGameplayHUD gameplayHUD)
        {
            _gameplayHUD = gameplayHUD;
        }
        
        public void Enter()
        {
           _gameplayHUD.SetChangePlayerAvailability(true); 
        }
        
        public void Exit()
        {
            _gameplayHUD.SetChangePlayerAvailability(false); 
        }
    }
}