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

        public void Enter(PlayerFacade opponent)
        {
            _player.FocusOn(opponent.transform);

            if (_player.IsPlayable)
                _player.PrioritizeCamera();
            
            //play fight for ball animation
        }

        public void Exit()
        {
        }
    }
}