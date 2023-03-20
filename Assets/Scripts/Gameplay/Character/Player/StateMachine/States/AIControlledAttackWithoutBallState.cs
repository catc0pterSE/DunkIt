using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class AIControlledAttackWithoutBallState: IParameterlessState
    {
        private readonly PlayerFacade _player;

        public AIControlledAttackWithoutBallState(PlayerFacade player)
        {
            _player = player;
        }
        
        public void Enter()
        {
           _player.EnableAIControlledAttackWithoutBallBrain();
           _player.EnableCharacterController();
        }

        public void Exit()
        {
          _player.DisableAIControlledAttackWithoutBallBrain();
          _player.DisableCharacterController();
        }
    }
}