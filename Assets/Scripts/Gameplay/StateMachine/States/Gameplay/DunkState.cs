using Cinemachine;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.Transitions;
using Modules.StateMachine;
using Scene;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.Gameplay
{
    using Ball.MonoBehavior;
    public class DunkState : StateWithTransitions, IParameterState<PlayerFacade>
    {
        private readonly Ball _ball;

        private CinemachineVirtualCamera _dunkVirtualCamera;

        public DunkState(PlayerFacade[] leftTeam, PlayerFacade[] rightTeam, SceneInitials sceneInitials, Ball ball, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _ball = ball;

            Transitions = new ITransition[]
            {
                new AnyToBallChasingStateTransition(ball, gameplayLoopStateMachine),
            };
        }

        public void Enter(PlayerFacade dunkingPlayer)
        {
            base.Enter();
            _dunkVirtualCamera = dunkingPlayer.OppositeRing.DunkVirtualCamera;
            SetUpDunkCamera();
            dunkingPlayer.EnterDunkState();
        }

        private void SetUpDunkCamera()
        {
            _dunkVirtualCamera.Prioritize();
            _dunkVirtualCamera.LookAt = _ball.transform;
        }
    }
}