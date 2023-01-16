using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Cutscene;
using Modules.StateMachine;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    using Ball.MonoBehavior;

    public abstract class CutsceneState : IParameterlessState
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly EnemyFacade[] _enemyTeam;
        private readonly ICutscene _cutscene;

        protected CutsceneState
        (
            PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam,
            ICutscene cutscene
        )
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _cutscene = cutscene;
        }


        public virtual void Enter()
        {
            SetCharactersStates();
            SubscribeOnCutscene();
            EnableCutscene();
            LaunchCutscene();
        }

        public void Exit()
        {
            DisableCutscene();
        }

        protected abstract void EnterNextState();

        private void SetCharactersStates()
        {
            _playerTeam.Map(player => player.StateMachine.Enter<Character.Player.StateMachine.States.CutsceneState>());
            _enemyTeam.Map(enemy => enemy.StateMachine.Enter<Character.NPC.EnemyPlayer.StateMachine.States.CutsceneState>());
        }
        
        private void SubscribeOnCutscene() =>
            _cutscene.Finished += EnterNextState;

        private void LaunchCutscene()
        {
            _cutscene.Run();
        }
            
        private void EnableCutscene() =>
            _cutscene.Enable();

        private void DisableCutscene() =>
            _cutscene.Disable();
    }
}