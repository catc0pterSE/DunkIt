using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Minigame;
using Modules.StateMachine;
using UI.HUD;

namespace Gameplay.StateMachine.States.MinigameStates
{
    using Ball.MonoBehavior;
    public class BallContestState: MinigameState, IParameterState<(PlayerFacade, PlayerFacade)>
    {
        public BallContestState(Ball ball, IGameplayHUD gameplayHUD) : base(gameplayHUD)
        {
        }

        public void Enter((PlayerFacade, PlayerFacade) payLoad)
        {
        }

        protected override IMinigame Minigame { get; }

        protected override void InitializeMinigame()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnMiniGameWon()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnMiniGameLost()
        {
            throw new System.NotImplementedException();
        }

        protected override void SetCharactersStates()
        {
            throw new System.NotImplementedException();
        }

      
    }
}