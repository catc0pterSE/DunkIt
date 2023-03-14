﻿using System;
using Cinemachine;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Minigame;
using Gameplay.Minigame.Throw;
using Gameplay.StateMachine.States.CutsceneStates;
using Gameplay.StateMachine.States.Gameplay;
using Gameplay.StateMachine.Transitions;
using Infrastructure.CoroutineRunner;
using Infrastructure.Factory;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;
using Scene;
using Scene.Ring;
using UI;
using UI.HUD;
using UnityEngine;
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
        private Ring _ring;

        public ThrowState(
            IGameplayHUD gameplayHUD,
            SceneConfig sceneConfig,
            GameplayLoopStateMachine gameplayLoopStateMachine,
            PlayerFacade[] enemyTeam,
            Ball.MonoBehavior.Ball ball,
            LoadingCurtain loadingCurtain,
            ICoroutineRunner coroutineRunner,
            IGameObjectFactory gameObjectFactory
        ) : base(gameplayHUD)
        {
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
            _ball = ball;
            _loadingCurtain = loadingCurtain;
            _enemyTeam = enemyTeam;
            _sceneConfig = sceneConfig;
            Transitions = new ITransition[]
                { new AnyToFightForBallTransition(ball, gameplayLoopStateMachine, coroutineRunner, false) };
            _throwMinigame = gameObjectFactory.CreateThrowMinigame();
        }

        public void Enter(PlayerFacade player)
        {
            SetThrowingPlayer(player);
            _ring = _throwingPlayer.OppositeRing;
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

        protected override void OnMiniGameWon() =>
            MoveToCelebrateCutsceneState();

        protected override void OnMiniGameLost()
        {
            _loadingCurtain.FadeInFadeOut(() =>
            {
                SetDropBall();
                MoveToGameplayState();
            });
        }

        protected override void SetCharactersStates() =>
            _throwingPlayer.EnterThrowState();

        private void SetDropBall()
        {
            PlayerFacade primaryEnemy = _enemyTeam[NumericConstants.PrimaryTeamMemberIndex];
            primaryEnemy.transform.position = _sceneConfig.EnemyDropBallPoint.position;
            _ball.SetOwner(primaryEnemy);
        }

        private void MoveToCelebrateCutsceneState() =>
            _gameplayLoopStateMachine.Enter<CelebrateCutsceneState>();

        private void MoveToGameplayState() =>
            _gameplayLoopStateMachine.Enter<GameplayState>();
    }
}