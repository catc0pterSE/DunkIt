using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled;
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
            _player.EnableCharacterController();
            _player.EnableFightForBallTriggerZone();
            _player.EnableDistanceTracker();

            if (_player.CanBeLocalControlled)
                EnableInputControlledPreset();

            if (_player.IsAIOnlyControlled)
                EnableAIControlledPreset();
        }

        private void EnableInputControlledPreset()
        {
            _playerService.Set(_player);
            _player.EnableLocalControlledBrain();
            _player.SubscribeOnDunkInput();
            _player.SubscribeOnPassInput();
            _player.SubscribeOnThrowInput();
            _player.PrioritizeCamera();
            _player.FocusOn(_player.OppositeRing.transform);
        }

        private void EnableAIControlledPreset() =>
            _player.EnableAIControlledBrain(AI.AttackWithBall);

        public void Exit()
        {
            _player.DisableCharacterController();
            _player.DisableLocalControlledBrain();
            _player.UnsubscribeFromDunkInput();
            _player.UnsubscribeFromPassInput();
            _player.UnsubscribeFromThrowInput();
            _player.DisableAIControlledBrain();
            _player.DisableFightForBallTriggerZone();
            _player.DisableDistanceTracker();
        }
    }
}