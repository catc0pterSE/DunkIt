using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class AIControlledAttackWithBallState : IParameterlessState
    {
        private readonly PlayerFacade _player;

        public AIControlledAttackWithBallState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.EnableAIControlledAttackWithBallBrain();
            _player.EnableCharacterController();
        }



        public void Exit()
        {
            _player.DisableAIControlledAttackWithBallBrain();
            _player.DisableCharacterController();
        }
           
    }
}