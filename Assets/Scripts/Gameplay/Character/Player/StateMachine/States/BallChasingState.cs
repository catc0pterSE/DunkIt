using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled;
using Infrastructure.PlayerService;
using Modules.StateMachine;
using UI.HUD.StateMachine.States;

namespace Gameplay.Character.Player.StateMachine.States
{
    using Ball.MonoBehavior;
    
    public class BallChasingState : IParameterlessState
    {
        private readonly PlayerFacade _player;
        private readonly IPlayerService _playerService;
        private readonly Ball _ball;

        public BallChasingState(PlayerFacade player, IPlayerService playerService, Ball ball)
        {
            _player = player;
            _playerService = playerService;
            _ball = ball;
        }

        public void Enter()
        {
            _player.EnableCharacterController();
            _player.EnableCatcher();

            if (_player.CanBeLocalControlled && _playerService.CurrentControlled == _player)
                EnableLocalControlledPreset();
            else
                EnableAIControlledPreset();
        }

        private void EnableLocalControlledPreset()
        {
            _playerService.Set(_player);
            _player.EnableLocalControlledBrain();
            _player.SubscribeOnChangePlayerInput();
            _player.PrioritizeCamera();
            _player.FocusOn(_ball.transform);
        }

        private void EnableAIControlledPreset() =>
            _player.EnableAIControlledBrain(AI.ChasingBall);

        public void Exit()
        {
            _player.DisableAIControlledBrain();
            _player.DisableCharacterController();
            _player.UnsubscribeFromChangePlayerInput();
            _player.DisableLocalControlledBrain();
            _player.DisableCatcher();
        }
    }
}