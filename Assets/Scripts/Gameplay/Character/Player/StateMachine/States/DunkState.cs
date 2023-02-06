using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using Scene.Ring;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class DunkState: IParameterState<Ring>
    {
        private readonly PlayerFacade _player;

        private Ring _ring;
        
        public DunkState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter(Ring ring)
        {
            _player.DisableInputControlledBrain();
            _player.EnableDunker();
            _player.Dunk(ring);
        }

        public void Exit()
        {
            _player.DisableDunker();
        }
    }
}