using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class AIControlledDefenceState: IParameterlessState
    {
        private readonly PlayerFacade _player;

        public AIControlledDefenceState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.EnableCharacterController();
            _player.EnableAIControlledDefenceBrain();
        }
        
        public void Exit()
        {
            _player.DisableCharacterController();
            _player.DisableAIControlledDefenceBrain();
           
        }
    }
}