using Cinemachine;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.MinigameStates;
using Infrastructure.Factory;
using Infrastructure.Input.InputService;
using Modules.StateMachine;
using UI.HUD;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    public class StartCutsceneState : CutsceneState, IParameterlessState
    {
        private readonly Referee _referee;
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public StartCutsceneState
        (PlayerFacade[] playerTeam,
            PlayerFacade[] enemyTeam,
            Referee referee,
            CinemachineBrain camera,
            Ball.MonoBehavior.Ball ball,
            IGameplayHUD gameplayHUD,
            GameplayLoopStateMachine gameplayLoopStateMachine,
            IGameObjectFactory gameObjectFactory, 
            IInputService inputService
        ) : base
        (
            playerTeam,
            enemyTeam,
            gameplayHUD,
            gameObjectFactory.CreateStartCutscene().Initialize(camera, playerTeam, enemyTeam, referee, inputService)
        )
        {
            _referee = referee;
            _ball = ball;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public override void Enter()
        {
            base.Enter();
            _referee.Enable();
            _ball.SetOwner(_referee);
        }

        public override void Exit()
        {
            base.Exit();
            _referee.Disable();
        }

        protected override void EnterNextState()
        {
            _gameplayLoopStateMachine.Enter<JumpBallState>();
        }
    }
}