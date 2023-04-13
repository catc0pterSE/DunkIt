﻿using System;
using System.Linq;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Minigame;
using Gameplay.Minigame.FightForBall;
using Gameplay.StateMachine.Transitions;
using Infrastructure.Factory;
using Modules.StateMachine;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.MinigameStates
{
    public class FightForBallState : MinigameState, IParameterState<PlayerFacade[]>
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly PlayerFacade[] _enemyTeam;
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly FightForBallMinigame _fightForBallMinigame;

        private PlayerFacade _fightingPlayer;
        private PlayerFacade _fightingEnemy;

        public FightForBallState(PlayerFacade[] playerTeam, PlayerFacade[] enemyTeam, Ball.MonoBehavior.Ball ball,
            GameplayLoopStateMachine gameplayLoopStateMachine, IGameObjectFactory gameObjectFactory)
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _ball = ball;
            _fightForBallMinigame = gameObjectFactory.CreateFightForBallMinigame();

            Transitions = new ITransition[]
            {
                new AnyToAttackDefenceStateTransition(ball, gameplayLoopStateMachine)
            };
        }

        public void Enter(PlayerFacade[] payload)
        {
            _fightingPlayer = payload.FindFirstOrNull(participant => participant.CanBeLocalControlled)
                              ?? throw new NullReferenceException(
                                  "Theres is no playable playable player in participants");
            _fightingEnemy = payload.FindFirstOrNull(participant => participant.CanBeLocalControlled == false)
                             ?? throw new NullReferenceException("Theres is no npc player in participants");
            base.Enter();
        }

        protected override IMinigame Minigame => _fightForBallMinigame;

        protected override void InitializeMinigame()
        {
        }

        protected override void OnMiniGameWon() =>
            _ball.SetOwner(_fightingPlayer);


        protected override void OnMiniGameLost() =>
            _ball.SetOwner(_fightingEnemy);


        protected override void SetCharactersStates()
        {
            _playerTeam.Union(_enemyTeam).Map(player => player.EnterIdleState(_ball.Owner.transform.position));
            _fightingPlayer.EnterFightForBallState(_fightingEnemy);
            _fightingEnemy.EnterFightForBallState(_fightingPlayer);
        }
    }
}