using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class AttackWithoutBallState : IParameterlessState
    {
        private readonly PlayerFacade _player;

        public AttackWithoutBallState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.EnableDistanceTracker();
            
            if (_player.CanBeLocalControlled || _player.IsAIOnlyControlled)
                EnableAIControlledPreset();
        }

        private void EnableAIControlledPreset()
        {
            _player.EnableAIControlledBrain(AI.AttackWithoutBall);
            _player.EnableCharacterController();
        }

        public void Exit()
        {
            _player.DisableDistanceTracker();
            _player.DisableAIControlledBrain();
            _player.DisableCharacterController();
        }
    }
}