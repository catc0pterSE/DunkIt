using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class ThrowState : IParameterlessState
    {
        private readonly PlayerFacade _player;

        public ThrowState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            if (_player.CanBeLocalControlled)
                _player.EnableLocalControlledBallThrower();
            else
                _player.EnableAIControlledBallThrower();
        }

        public void Exit()
        {
            _player.DisableLocalControlledBallThrower();
            _player.DisableAIControlledBallThrower();
        }
    }
}