using Gameplay.Minigame;
using Modules.StateMachine;
using UI.HUD;

namespace Gameplay.StateMachine.States.MinigameStates
{
    public abstract class MinigameState : StateWithTransitions
    {
        private readonly IGameplayHUD _gameplayHUD;
        protected abstract IMinigame Minigame { get; }

        protected MinigameState(IGameplayHUD gameplayHUD)
        {
            
            _gameplayHUD = gameplayHUD;
        }

        public override void Enter()
        {
            base.Enter();
            SetCharactersStates();
            InitializeMinigame();
            DisableGameplayHUD();
            SubscribeOnMinigame();
            EnableMinigame();
            LaunchMinigame();
        }

        protected abstract void InitializeMinigame();
        protected abstract void OnMiniGameWon();
        protected abstract void OnMiniGameLost();
        protected abstract void SetCharactersStates();

        private void DisableGameplayHUD() =>
            _gameplayHUD.Disable();


        public override void Exit()
        {
            base.Exit();
            UnsubscribeFromMinigame();
            DisableMinigame();
        }

        private void SubscribeOnMinigame()
        {
            Minigame.Won += OnMiniGameWon;
            Minigame.Lost += OnMiniGameLost;
        }

        private void UnsubscribeFromMinigame()
        {
            Minigame.Won -= OnMiniGameWon;
            Minigame.Lost -= OnMiniGameLost;
        }

        private void LaunchMinigame()
        {
            Minigame.Launch();
        }

        private void EnableMinigame() =>
            Minigame.Enable();

        private void DisableMinigame() =>
            Minigame.Disable();
    }
}