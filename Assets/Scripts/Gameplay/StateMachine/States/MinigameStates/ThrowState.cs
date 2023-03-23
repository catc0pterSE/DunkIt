using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Minigame;
using Gameplay.Minigame.Throw;
using Gameplay.StateMachine.States.CutsceneStates;
using Gameplay.StateMachine.Transitions;
using Infrastructure.CoroutineRunner;
using Infrastructure.Factory;
using Modules.StateMachine;
using Scene;
using UI;
using UI.HUD;

namespace Gameplay.StateMachine.States.MinigameStates
{
    public class ThrowState : MinigameState, IParameterState<PlayerFacade>
    {
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly ThrowMinigame _throwMinigame;
        private PlayerFacade _throwingPlayer;
        
        public ThrowState(
            IGameplayHUD gameplayHUD,
            SceneConfig sceneConfig,
            GameplayLoopStateMachine gameplayLoopStateMachine,
            PlayerFacade[] playerTeam,
            PlayerFacade[] enemyTeam,
            Ball.MonoBehavior.Ball ball,
            LoadingCurtain loadingCurtain,
            ICoroutineRunner coroutineRunner,
            IGameObjectFactory gameObjectFactory
        ) : base(gameplayHUD)
        {
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
            _ball = ball;
            Transitions = new ITransition[]
            {
                /*new AnyToFightForBallTransition(ball, gameplayLoopStateMachine, coroutineRunner, false), */ 
                new AnyToDropBallTransition(ball, gameplayLoopStateMachine, loadingCurtain, playerTeam, enemyTeam, sceneConfig)
            };
            _throwMinigame = gameObjectFactory.CreateThrowMinigame();
        }

        public void Enter(PlayerFacade player)
        {
            SetThrowingPlayer(player);
            base.Enter();
        }

        private void SetThrowingPlayer(PlayerFacade player) =>
            _throwingPlayer = player;

        protected override IMinigame Minigame => _throwMinigame;

        protected override void InitializeMinigame()
        {
            _throwMinigame.Initialize
            (
                _throwingPlayer,
                _ball
            );
        }

        protected override void OnMiniGameWon() => MoveToCelebrateCutsceneState();

        protected override void OnMiniGameLost() { } //TODO: ???

        protected override void SetCharactersStates() =>
            _throwingPlayer.EnterThrowState();

        private void MoveToCelebrateCutsceneState() =>
            _gameplayLoopStateMachine.Enter<CelebrateCutsceneState>();
    }
}