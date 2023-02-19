using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class InputControlledAttackState : IParameterlessState
    {
        private readonly MonoBehaviour.PlayerFacade _player;

        public InputControlledAttackState(MonoBehaviour.PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.EnableInputControlledBrain();
            _player.EnablePlayerMover();
            _player.PrioritizeCamera();
            _player.FocusOnEnemyBasket();
            _player.EnableDistanceTracker();
            _player.EnableFightForBallTriggerZone();
        }
        
        public void Exit()
        {
            _player.DisablePlayerMover();
            _player.DisableInputControlledBrain();
            _player.DisableDistanceTracker();
            _player.DisableBallContestTriggerZone();
        }
    }
}