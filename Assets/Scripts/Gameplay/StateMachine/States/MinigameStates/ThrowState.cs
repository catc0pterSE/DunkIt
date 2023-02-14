using System;
using Cinemachine;
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
using UnityEngine;
using Utility.Constants;

namespace Gameplay.StateMachine.States.MinigameStates
{
    public class ThrowState : MinigameState, IParameterState<PlayerFacade>
    {
        private const float DefaultCameraXOffset = 10;
        private const int CameraTargetGroupPlayerIndex = 1;

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
            SetupCamera();
            base.Enter();
        }

        private void SetupCamera()
        {
            Transform playerTransform = _throwingPlayer.transform;
            Transform ringTransform = _sceneConfig.EnemyRing.transform;
            CinemachineVirtualCamera ringCamera = _sceneConfig.EnemyRing.VirtualCamera;
            _sceneConfig.EnemyRing.RingTargetGroup.m_Targets[CameraTargetGroupPlayerIndex].target = playerTransform;
            ringCamera.Follow = playerTransform;
            CinemachineFramingTransposer framingTransposer =
                ringCamera.GetCinemachineComponent<CinemachineFramingTransposer>()
                ?? throw new NullReferenceException("There is nщ farmingTransposer on RingVirtualCamera");
            framingTransposer.m_TrackedObjectOffset.x =
                ringTransform.position.z > playerTransform.position.z ? -DefaultCameraXOffset : DefaultCameraXOffset;
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