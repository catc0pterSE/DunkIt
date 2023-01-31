using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Minigame;
using UI.HUD;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.MinigameStates
{
    public abstract class MinigameState 
    {
        private readonly IGameplayHUD _gameplayHUD;
        
        protected PlayerFacade[] PlayerTeam;
        protected  EnemyFacade[] EnemyTeam;
        protected IMinigame Minigame;

        public MinigameState(PlayerFacade[] playerTeam, EnemyFacade[] enemyTeam, IGameplayHUD gameplayHUD)
        {
            PlayerTeam = playerTeam;
            EnemyTeam = enemyTeam;
            _gameplayHUD = gameplayHUD;
        }

        public virtual void Enter()
        {
            InitializeMinigame();
            DisableGameplayHUD();
            SetCharactersStates();
            SubscribeOnMinigame();
            EnableMinigame();
            LaunchMinigame();
        }

        protected abstract void InitializeMinigame();

        private void DisableGameplayHUD() =>
            _gameplayHUD.Disable();


        public virtual void Exit()
        {
            UnsubscribeFromMinigame();
            DisableMinigame();
        }

        protected abstract void OnMiniGameWon();
        protected abstract void OnMiniGameLost();

        private void SetCharactersStates()
        {
            PlayerTeam.Map(player =>
                player.StateMachine.Enter<Character.Player.StateMachine.States.NotControlledState>());
            EnemyTeam.Map(enemy =>
                enemy.StateMachine.Enter<Character.NPC.EnemyPlayer.StateMachine.States.NotControlledState>());
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