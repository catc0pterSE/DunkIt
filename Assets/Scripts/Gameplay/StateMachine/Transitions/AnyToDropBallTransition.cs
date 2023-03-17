using System;
using Gameplay.Character;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Modules.StateMachine;
using Scene;
using UI;
using UnityEngine;
using Utility.Constants;

namespace Gameplay.StateMachine.Transitions
{
    public class AnyToDropBallTransition : ITransition
    {
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly PlayerFacade[] _playerTeam;
        private readonly PlayerFacade[] _enemyTeam;
        private readonly SceneConfig _sceneConfig;

        public AnyToDropBallTransition(Ball.MonoBehavior.Ball ball, GameplayLoopStateMachine gameplayLoopStateMachine,
            LoadingCurtain loadingCurtain, PlayerFacade[] playerTeam, PlayerFacade[] enemyTeam, SceneConfig sceneConfig)
        {
            _ball = ball;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
            _loadingCurtain = loadingCurtain;
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _sceneConfig = sceneConfig;
        }

        public void Enable()
        {
            _ball.Lost += OnBallLost;
        }
        
        public void Disable()
        {
            _ball.Lost -= OnBallLost;
        }

        private void OnBallLost(CharacterFacade lostCharacter)
        {
            if (lostCharacter is not PlayerFacade lostPlayer)
                throw new Exception("Ball is lost by someone who is not basketball player");

            _loadingCurtain.FadeInFadeOut(
                () =>
                {
                    SetDropBall(lostPlayer);
                    EnterGameplayState();
                });
        }

        private void SetDropBall(PlayerFacade lostPlayer)
        {
            PlayerFacade droppingPlayer;
            Vector3 dropPosition;

            if (lostPlayer.LeftSide)
            {
                droppingPlayer = _enemyTeam[NumericConstants.PrimaryTeamMemberIndex];
                dropPosition = _sceneConfig.LeftDropBallPoint.transform.position;
            }
            else
            {
                droppingPlayer = _playerTeam[NumericConstants.PrimaryTeamMemberIndex];
                dropPosition = _sceneConfig.RightDropBallPoint.transform.position;
            }

            droppingPlayer.transform.position = dropPosition;
            _ball.SetOwner(droppingPlayer);
        }

        private void EnterGameplayState() => _gameplayLoopStateMachine.Enter<GameplayState>();
    }
}