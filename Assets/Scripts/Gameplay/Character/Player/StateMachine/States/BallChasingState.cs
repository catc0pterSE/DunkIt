using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled;
using Gameplay.Character.Player.MonoBehaviour.Brains.LocalControlled;
using Infrastructure.PlayerService;
using Modules.StateMachine;

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
            _player.EnableMover();
            _player.EnableCatcher();

            if (_playerService.CurrentControlled == _player)
                EnableLocalControlledPreset();
            else
                EnableAIControlledPreset();
        }

        private void EnableLocalControlledPreset()
        {
            SubscribeOnChangePlayerInput();
            _player.EnableLocalControlledBrain(new [] { LocalAction.Move , LocalAction.ChangePlayer, LocalAction.Rotate});
            _player.PrioritizeCamera();
            _player.FocusOn(_ball.transform);
        }
        
        private void SubscribeOnChangePlayerInput() =>
            _player.ChangePlayerInitiated += OnChangePlayerInitiated;

        private void UnsubscribeFromChangePlayerInput() =>
            _player.ChangePlayerInitiated -= OnChangePlayerInitiated;
        
        private void OnChangePlayerInitiated(PlayerFacade nextPlayer)
        {
            _playerService.Set(nextPlayer);
            _player.EnterBallChasingState();
            nextPlayer.EnterBallChasingState();
        }

        private void EnableAIControlledPreset() =>
            _player.EnableAIControlledBrain(AI.ChasingBall);

        public void Exit()
        {
            _player.DisableAIControlledBrain();
            _player.DisableMover();
            _player.DisableLocalControlledBrain();
            _player.DisableCatcher();
            UnsubscribeFromChangePlayerInput();
        }
    }
}