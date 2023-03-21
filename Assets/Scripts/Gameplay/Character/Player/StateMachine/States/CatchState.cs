using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class CatchState : IParameterState<PlayerFacade>
    {
        private readonly PlayerFacade _player;

        public CatchState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter(PlayerFacade passingPlayer)
        {
            _player.FocusOn(passingPlayer.transform);
            _player.RotateTo(passingPlayer.transform.position);
            _player.EnableCatcher();
        }

        public void Exit()
        {
            _player.DisableCatcher();
        }
    }
}