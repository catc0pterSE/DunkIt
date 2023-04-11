using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled;
using Infrastructure.PlayerService;
using Modules.StateMachine;
using UI.HUD.StateMachine.States;

namespace Gameplay.Character.Player.StateMachine.States
{
    using Ball.MonoBehavior;

    public class DefenceState : IParameterlessState
    {
        private readonly PlayerFacade _player;
        private readonly IPlayerService _playerService;
        private readonly Ball _ball;

        public DefenceState(PlayerFacade player, IPlayerService playerService, Ball ball)
        {
            _player = player;
            _playerService = playerService;
            _ball = ball;
        }

        public void Enter()
        {
            _player.EnableCharacterController();

            if (_player.CanBeLocalControlled && _playerService.CurrentControlled == _player)
                EnterInputControlledPreset();
            else
                EnterAIControlledPreset();
        }

        public void Exit()
        {
            _player.DisableCharacterController();
            _player.DisableLocalControlledBrain();
            _player.UnsubscribeFromChangePlayerInput();
            _player.DisableAIControlledBrain();
        }

        private void EnterInputControlledPreset()
        {
            _playerService.Set(_player);
            _player.EnableLocalControlledBrain();
            _player.SubscribeOnChangePlayerInput();
            _player.PrioritizeCamera();
            _player.FocusOn(_ball.Owner.transform);
        }

        private void EnterAIControlledPreset() =>
            _player.EnableAIControlledBrain(AI.Defence);
    }
}