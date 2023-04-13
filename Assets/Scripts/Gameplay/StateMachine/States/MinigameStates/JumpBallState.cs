using System.Linq;
using Cinemachine;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Minigame;
using Gameplay.Minigame.JumpBall;
using Gameplay.StateMachine.Transitions;
using Infrastructure.Factory;
using Infrastructure.Input.InputService;
using Modules.StateMachine;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.MinigameStates
{
    public class JumpBallState : MinigameState, IParameterlessState
    {
        private readonly PlayerFacade[] _rightTeam;
        private readonly PlayerFacade[] _leftTeam;
        private readonly PlayerFacade _playablePlayer;
        private readonly PlayerFacade _notPlayablePlayer;
        private readonly Referee _referee;
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly CinemachineBrain _gameplayCamera;
        private readonly IInputService _inputService;
        private readonly JumpBallMinigame _jumpBallMinigame;

        public JumpBallState
        (
            PlayerFacade[] leftTeam,
            PlayerFacade[] rightTeam,
            PlayerFacade[] playableTeam,
            PlayerFacade[] notPlayableTeam,
            Referee referee,
            Ball.MonoBehavior.Ball ball,
            CinemachineBrain gameplayCamera,
            GameplayLoopStateMachine gameplayLoopStateMachine,
            IGameObjectFactory gameObjectFactory,
            IInputService inputService
        )
        {
            Transitions = new ITransition[]
            {
                new AnyToAttackDefenceStateTransition(ball, gameplayLoopStateMachine),
            };

            _rightTeam = rightTeam;
            _leftTeam = leftTeam;
            _playablePlayer = playableTeam.First();
            _notPlayablePlayer = notPlayableTeam.First();
            _referee = referee;
            _ball = ball;
            _gameplayCamera = gameplayCamera;
            _inputService = inputService;
            _jumpBallMinigame = gameObjectFactory.CreateJumpBallMinigame();
        }

        protected override IMinigame Minigame => _jumpBallMinigame;


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

        protected override void InitializeMinigame() =>
            _jumpBallMinigame.Initialize(_leftTeam, _rightTeam, _gameplayCamera, _referee, _ball, _inputService); //TODO: adjust difficulty based on PlayerData

        protected override void OnMiniGameWon() =>
            _ball.SetOwner(_playablePlayer);

        protected override void OnMiniGameLost() =>
            _ball.SetOwner(_notPlayablePlayer);


        protected override void SetCharactersStates()
        {
            _leftTeam.Map(player => player.EnterNotControlledState());
            _rightTeam.Map(player => player.EnterNotControlledState());
        }
    }
}