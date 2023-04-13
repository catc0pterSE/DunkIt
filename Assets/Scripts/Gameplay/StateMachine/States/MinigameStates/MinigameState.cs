using Gameplay.Minigame;
using Modules.StateMachine;

namespace Gameplay.StateMachine.States.MinigameStates
{
    public abstract class MinigameState : StateWithTransitions
    {
        protected abstract IMinigame Minigame { get; }
        public override void Enter()
        {
            base.Enter();
            SetCharactersStates();
            InitializeMinigame();
            SubscribeOnMinigame();
            EnableMinigame();
            LaunchMinigame();
        }

        protected abstract void InitializeMinigame();
        protected abstract void OnMiniGameWon();
        protected abstract void OnMiniGameLost();
        protected abstract void SetCharactersStates();

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