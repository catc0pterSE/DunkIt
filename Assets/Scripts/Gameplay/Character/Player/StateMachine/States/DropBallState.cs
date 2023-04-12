using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled;
using Gameplay.Character.Player.MonoBehaviour.Brains.LocalControlled;
using Infrastructure.PlayerService;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class DropBallState : IParameterlessState
    {
        private readonly PlayerFacade _player;
        private readonly IPlayerService _playerService;

        public DropBallState(PlayerFacade player, IPlayerService playerService)
        {
            _player = player;
            _playerService = playerService;
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
            _playerService.Set(_player);
            _player.FocusOn(_player.OppositeRing.transform);
            _player.EnableLocalControlledBrain(new [] { LocalAction.Pass, LocalAction.Rotate });
        }

        private void EnableAIControlledPreset()
        {
            _player.EnableAIControlledBrain(AI.DropBall);
        }

        public void Exit()
        {
            _player.DisableLocalControlledBrain();
            _player.DisableAIControlledBrain();
        }
    }
}