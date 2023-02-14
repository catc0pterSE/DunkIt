using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Minigame;
using Gameplay.Minigame.Throw;
using Gameplay.StateMachine.States.CutsceneStates;
using Gameplay.StateMachine.States.Gameplay;
using Gameplay.StateMachine.Transitions;
using Infrastructure.CoroutineRunner;
using Infrastructure.Factory;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;
using Scene;
using UI;
using UI.HUD;
using Utility.Constants;

namespace Gameplay.StateMachine.States.MinigameStates
{
    public class ThrowState : MinigameState, IParameterState<PlayerFacade>
    {
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly PlayerFacade[] _enemyTeam;
        private readonly SceneConfig _sceneConfig;
        private readonly ThrowMinigame _throwMinigame;

        private PlayerFacade _throwingPlayer;

        public ThrowState(
            IGameplayHUD gameplayHUD,
            SceneConfig sceneConfig,
            GameplayLoopStateMachine gameplayLoopStateMachine,
            PlayerFacade[] enemyTeam,
            Ball.MonoBehavior.Ball ball,
            LoadingCurtain loadingCurtain,
            ICoroutineRunner coroutineRunner) : base(gameplayHUD)
        {
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
            _ball = ball;
            _loadingCurtain = loadingCurtain;
            _enemyTeam = enemyTeam;
            _sceneConfig = sceneConfig;
            Transitions = new ITransition[]
                { new AnyToBallContestStateTransition(ball, gameplayLoopStateMachine, coroutineRunner) };
            _throwMinigame = Services.Container.Single<IGameObjectFactory>().CreateThrowMinigame();
        }

        public void Enter(PlayerFacade player)
        {
            SetThrowingPlayer(player);
            _sceneConfig.EnemyRing.RingTargetGroup.m_Targets[1].target = _throwingPlayer.transform;
            // _sceneConfig.EnemyRing.VirtualCamera.Follow = _throwingPlayer.transform; TODO: remove. test
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
                _enemyTeam[NumericConstants.PrimaryTeamMemberIndex],
                _sceneConfig,
                _ball,
                _loadingCurtain
            );
        }

        protected override void OnMiniGameWon() =>
            MoveToCelebrateCutsceneState();

        protected override void OnMiniGameLost() =>
            MoveToGameplayState();

        protected override void SetCharactersStates() =>
            _throwingPlayer.EnterThrowState(_sceneConfig.EnemyRing.transform.position);

        private void MoveToCelebrateCutsceneState() =>
            _gameplayLoopStateMachine.Enter<CelebrateCutsceneState>();

        private void MoveToGameplayState() =>
            _gameplayLoopStateMachine.Enter<GameplayState>();
    }
}