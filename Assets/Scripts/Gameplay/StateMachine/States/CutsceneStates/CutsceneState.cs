using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Cutscene;
using Modules.StateMachine;
using UI.HUD;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    public abstract class CutsceneState : StateWithTransitions
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly PlayerFacade[] _enemyTeam;
        private readonly IGameplayHUD _gameplayHUD;
        private readonly ICutscene _cutscene;

        protected CutsceneState
        (
            PlayerFacade[] playerTeam,
            PlayerFacade[] enemyTeam,
            IGameplayHUD gameplayHUD,
            ICutscene cutscene
        )
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _gameplayHUD = gameplayHUD;
            _cutscene = cutscene;
        }


        public override void Enter()
        {
            base.Enter();
            DisableGameplayHUD();
            SetCharactersStates();
            SubscribeOnCutscene();
            EnableCutscene();
            LaunchCutscene();
        }

        private void DisableGameplayHUD()
        {
            _gameplayHUD.Disable();
        }

        public override void Exit()
        {
            base.Enter();
            DisableCutscene();
            UnsubscribeFromCutscene();
        }

        protected abstract void EnterNextState();

        private void SetCharactersStates()
        {
            _playerTeam.Map(player =>
                player.EnterNotControlledState());
            _enemyTeam.Map(enemy =>
                enemy.EnterNotControlledState());
        }

        private void SubscribeOnCutscene() =>
            _cutscene.Finished += EnterNextState;

        private void UnsubscribeFromCutscene() =>
            _cutscene.Finished -= EnterNextState;

        private void LaunchCutscene() =>
            _cutscene.Run();

        private void EnableCutscene() =>
            _cutscene.Enable();

        private void DisableCutscene() =>
            _cutscene.Disable();
    }
}