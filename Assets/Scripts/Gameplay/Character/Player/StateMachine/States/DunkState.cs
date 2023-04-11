using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using Scene.Ring;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class DunkState: IParameterlessState
    {
        private readonly PlayerFacade _player;

        private Ring _ring;
        
        public DunkState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.Dunk();
        }

        public void Exit() { }
    }
}