using Cysharp.Threading.Tasks.Triggers;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Brains.LocalControlled;
using Infrastructure.PlayerService;
using Modules.StateMachine;
using UnityEngine;

namespace Gameplay.Character.Player.StateMachine.States
{
    using Ball.MonoBehavior;
    public class IdleState : IParameterState<Vector3>
    {
        private readonly PlayerFacade _player;
        private readonly IPlayerService _playerService;
        private Vector3 _lookingAt;
        private Ball _ball;

        public IdleState(PlayerFacade player, IPlayerService playerService, Ball ball)
        {
            _ball = ball;
            _player = player;
            _playerService = playerService;
        }

        public void Enter(Vector3 lookingAt)
        {
            _player.RotateTo(lookingAt, () =>
            {
                if (_playerService.CurrentControlled == _player)
                {
                    _player.PrioritizeCamera();
                    _player.FocusOn(_ball.Owner.transform);
                }
            });
        }

        public void Exit()
        {
        }
    }
}