using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class ControlledDefenceState: IParameterlessState
    {
        private readonly PlayerFacade _player;

        public ControlledDefenceState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.EnableInputControlledBrain();
            _player.EnablePlayerMover();
            _player.PrioritizeCamera();
            _player.FocusOnBall();
        }
        
        public void Exit()
        {
            _player.DisablePlayerMover();
            _player.DisableInputControlledBrain();
            _player.DeprioritizeCamera();
        }
    }
}