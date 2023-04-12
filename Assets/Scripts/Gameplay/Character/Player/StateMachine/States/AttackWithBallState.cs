using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled;
using Gameplay.Character.Player.MonoBehaviour.Brains.LocalControlled;
using Infrastructure.PlayerService;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class AttackWithBallState : IParameterlessState
    {
        private readonly PlayerFacade _player;
        private readonly IPlayerService _playerService;

        public AttackWithBallState(PlayerFacade playerFacade, IPlayerService playerService)
        {
            _player = playerFacade;
            _playerService = playerService;
        }

        public void Enter()
        {
            _player.EnableMover();
            _player.EnableFightForBallTriggerZone();
            _player.EnableTargetTracker();

            if (_player.CanBeLocalControlled)
                EnableInputControlledPreset();

            if (_player.IsAIOnlyControlled)
                EnableAIControlledPreset();
        }

        private void EnableInputControlledPreset()
        {
            _playerService.Set(_player);
            _player.EnableLocalControlledBrain(new [] { LocalAction.Dunk , LocalAction.Pass, LocalAction.Throw, LocalAction.Move, LocalAction.Rotate});
            _player.PrioritizeCamera();
            _player.FocusOn(_player.OppositeRing.transform);
        }

        private void EnableAIControlledPreset() =>
            _player.EnableAIControlledBrain(AI.AttackWithBall);

        public void Exit()
        {
            _player.DisableMover();
            _player.DisableLocalControlledBrain();
            _player.DisableAIControlledBrain();
            _player.DisableFightForBallTriggerZone();
            _player.DisableTargetTracker();
        }
    }
}