using System;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Minigame;
using Gameplay.Minigame.FightForBall;
using Gameplay.StateMachine.States.Gameplay;
using Infrastructure.Factory;
using Modules.StateMachine;
using UI.HUD;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.MinigameStates
{
    public class FightForBallState : MinigameState, IParameterState<PlayerFacade[]>
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly PlayerFacade[] _enemyTeam;
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private readonly FightForBallMinigame _fightForBallMinigame;

        private PlayerFacade _fightingPlayer;
        private PlayerFacade _fightingEnemy;

        public FightForBallState(PlayerFacade[] playerTeam, PlayerFacade[] enemyTeam, Ball.MonoBehavior.Ball ball, IGameplayHUD gameplayHUD,
            GameplayLoopStateMachine gameplayLoopStateMachine, IGameObjectFactory gameObjectFactory)
            : base(gameplayHUD)
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _ball = ball;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
            _fightForBallMinigame = gameObjectFactory.CreateFightForBallMinigame();
        }

        public void Enter(PlayerFacade[] participants)
        {
            _fightingPlayer = participants.FindFirstOrNull(participant => participant.IsPlayable)
                      ?? throw new NullReferenceException("Theres is no playable player in participants");
            _fightingEnemy = participants.FindFirstOrNull(participant => participant.IsPlayable == false)
                     ?? throw new NullReferenceException("Theres is no enemy player in participants");
            base.Enter();
        }

        protected override IMinigame Minigame => _fightForBallMinigame;

        protected override void InitializeMinigame()
        {
            //TODO: adjust difficulty based on player stats from data layer
        }

        protected override void OnMiniGameWon()=>
            _ball.SetOwnerSmoothly(_fightingPlayer, EnterGameplayState);

        protected override void OnMiniGameLost()=>
            _ball.SetOwnerSmoothly(_fightingEnemy, EnterGameplayState);
            

        protected override void SetCharactersStates()
        {
            _playerTeam.Map(player => player.EnterIdleState());
            _enemyTeam.Map(enemy => enemy.EnterIdleState());
            _fightingPlayer.EnterFightForBallState(_fightingEnemy);
            _fightingEnemy.EnterFightForBallState(_fightingPlayer);
        }

        private void EnterGameplayState()
        {
            _gameplayLoopStateMachine.Enter<GameplayState>();
        }
            
    }
}