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
            _player.EnableMover();
            _player.EnableTargetTracker();
            _player.EnableAIControlledBrain(AI.AttackWithoutBall);
        }

        public void Exit()
        {
            _player.DisableTargetTracker();
            _player.DisableAIControlledBrain();
            _player.DisableMover();
        }
    }
}