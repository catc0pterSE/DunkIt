using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class PassState : IParameterState<PlayerFacade>
    {
        private readonly PlayerFacade _player;

        public PassState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter(PlayerFacade passTarget)
        {
            _player.FocusOn(passTarget.transform);
            _player.RotateTo(passTarget.transform.position, ()=> _player.Pass(passTarget));
        }

        public void Exit() {}
    }
}