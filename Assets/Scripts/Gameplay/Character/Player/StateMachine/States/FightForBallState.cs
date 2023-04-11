using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class FightForBallState : IParameterState<PlayerFacade>
    {
        private readonly PlayerFacade _player;

        public FightForBallState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter(PlayerFacade otherPlayer)
        {
            _player.FocusOn(otherPlayer.transform);

            if (_player.CanBeLocalControlled)
                _player.PrioritizeCamera();
        }

        public void Exit()
        {
        }
    }
}