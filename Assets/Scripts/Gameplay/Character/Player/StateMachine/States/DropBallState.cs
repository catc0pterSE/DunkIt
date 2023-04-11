using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class DropBallState : IParameterlessState
    {
        private readonly PlayerFacade _player;

        public DropBallState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            if (_player.CanBeLocalControlled)
                EnableLocalControlledPreset();
            else
                EnableAIControlledPreset();
        }

        private void EnableLocalControlledPreset()
        {
            _player.PrioritizeCamera();
            _player.EnableLocalControlledBallDropper();
        }

        private void EnableAIControlledPreset()
        {
            _player.EnableAIControlledBallDropper();
        }

        public void Exit()
        {
            _player.DisableLocalControlledBallDropper();
            _player.DisableAIControlledBallDropper();
        }
    }
}