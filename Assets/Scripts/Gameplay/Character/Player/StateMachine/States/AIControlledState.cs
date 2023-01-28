using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class AIControlledState : IParameterlessState
    {
        private readonly MonoBehaviour.PlayerFacade _player;

        public AIControlledState(MonoBehaviour.PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.EnableAIControlledBrain();
            _player.EnablePlayerMover();
            _player.DeprioritizeCamera();
        }


        public void Exit()
        {
            _player.DisableAIControlledBrain();
            _player.DisablePlayerMover();
        }
            
    }
}