using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Minigame;
using Modules.StateMachine;
using UI.HUD;
using UI.HUD.Mobile;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.MinigameStates
{
    public abstract class MinigameState : IParameterlessState
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly EnemyFacade[] _enemyTeam;
        private readonly IGameplayHUD _gameplayHUD;
        private readonly IMinigame _minigame;

        public MinigameState(PlayerFacade[] playerTeam, EnemyFacade[] enemyTeam, IGameplayHUD gameplayHUD,
            IMinigame minigame)
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _gameplayHUD = gameplayHUD;
            _minigame = minigame;
        }

        public virtual void Enter()
        {
            DisableGameplayHUD();
            SetCharactersStates();
            SubscribeOnMinigame();
            EnableMinigame();
            LaunchMinigame();
        }

        private void DisableGameplayHUD() =>
            _gameplayHUD.Disable();


        public virtual void Exit()
        {
            UnsubscribeOnMinigame();
            DisableMinigame();
        }

        protected abstract void OnMiniGameWon();
        protected abstract void OnMiniGameLost();

        private void SetCharactersStates()
        {
            _playerTeam.Map(player =>
                player.StateMachine.Enter<Character.Player.StateMachine.States.NotControlledState>());
            _enemyTeam.Map(enemy =>
                enemy.StateMachine.Enter<Character.NPC.EnemyPlayer.StateMachine.States.NotControlledState>());
        }

        private void SubscribeOnMinigame()
        {
            _minigame.Won += OnMiniGameWon;
            _minigame.Lost += OnMiniGameLost;
        }

        private void UnsubscribeOnMinigame()
        {
            _minigame.Won -= OnMiniGameWon;
            _minigame.Lost -= OnMiniGameLost;
        }

        private void LaunchMinigame()
        {
            _minigame.Launch();
        }

        private void EnableMinigame() =>
            _minigame.Enable();

        private void DisableMinigame() =>
            _minigame.Disable();
    }
}