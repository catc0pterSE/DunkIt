using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled;
using Gameplay.Character.Player.MonoBehaviour.Brains.LocalControlled;
using Infrastructure.PlayerService;
using Modules.StateMachine;
using NC_Custom_Tasks.Actions;
using UnityEngine;

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
            _player.EnableMover();

            if (_playerService.CurrentControlled == _player)
                EnterLocalControlledPreset();
            else
                EnterAIControlledPreset();
        }

        public void Exit()
        {
            _player.DisableMover();
            _player.DisableLocalControlledBrain();
            _player.DisableAIControlledBrain();
            UnsubscribeFromChangePlayerInput();
        }

        private void SubscribeOnChangePlayerInput() =>
            _player.ChangePlayerInitiated += OnChangePlayerInitiated;

        private void UnsubscribeFromChangePlayerInput() =>
            _player.ChangePlayerInitiated -= OnChangePlayerInitiated;
        
        private void OnChangePlayerInitiated(PlayerFacade nextPlayer)
        {
            _playerService.Set(nextPlayer);
            _player.EnterDefenceState();
            nextPlayer.EnterDefenceState();
        }

        private void EnterLocalControlledPreset()
        {
            _playerService.Set(_player);
            SubscribeOnChangePlayerInput();
            _player.EnableLocalControlledBrain(new [] { LocalAction.Move , LocalAction.ChangePlayer, LocalAction.Rotate});
            _player.PrioritizeCamera();
            _player.FocusOn(_ball.Owner.transform);
        }

        private void EnterAIControlledPreset() =>
            _player.EnableAIControlledBrain(AI.Defence);
    }
}