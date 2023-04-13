using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Cutscene;
using Modules.StateMachine;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    public abstract class CutsceneState : StateWithTransitions
    {
        private readonly PlayerFacade[] _players;
        private readonly ICutscene _cutscene;

        protected CutsceneState
        (
            PlayerFacade[] players,
            ICutscene cutscene
        )
        {
            _players = players;
            _cutscene = cutscene;
        }


        public override void Enter()
        {
            base.Enter();
            SetCharactersStates();
            SubscribeOnCutscene();
            EnableCutscene();
            LaunchCutscene();
        }

        public override void Exit()
        {
            base.Enter();
            DisableCutscene();
            UnsubscribeFromCutscene();
        }

        protected abstract void EnterNextState();

        private void SetCharactersStates() =>
            _players.Map(player => player.EnterNotControlledState());


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