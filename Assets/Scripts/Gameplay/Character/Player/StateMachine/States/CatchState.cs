using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class CatchState : IParameterlessState
    {
        private readonly PlayerFacade _player;

        public CatchState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.DisableAIControlledBrain();
            _player.DisableInputControlledBrain();
            _player.FocusOnBallOwner();
            _player.RotateToBallOwner();
            _player.EnableCatcher();
        }

        public void Exit()
        {
            _player.DisableCatcher();
        }
    }
}