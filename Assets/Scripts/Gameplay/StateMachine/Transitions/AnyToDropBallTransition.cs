using System;
using System.Linq;
using Gameplay.Character;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.Gameplay;
using Modules.StateMachine;
using Scene;
using UI;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;

namespace Gameplay.StateMachine.Transitions
{
    public class AnyToDropBallTransition : ITransition
    {
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly PlayerFacade[] _leftTeam;
        private readonly PlayerFacade[] _rightTeam;
        private readonly SceneConfig _sceneConfig;

        public AnyToDropBallTransition(Ball.MonoBehavior.Ball ball, GameplayLoopStateMachine gameplayLoopStateMachine,
            LoadingCurtain loadingCurtain, PlayerFacade[] playerTeam, PlayerFacade[] enemyTeam, SceneConfig sceneConfig)
        {
            _ball = ball;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
            _loadingCurtain = loadingCurtain;

            if (playerTeam.First().LeftSide)
            {
                _leftTeam = playerTeam;
                _rightTeam = enemyTeam;
            }
            else
            {
                _leftTeam = enemyTeam;
                _rightTeam = enemyTeam;
            }
            
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
            Transform dropPosition;

            if (lostPlayer.LeftSide)
            {
                droppingPlayer = _rightTeam[NumericConstants.PrimaryTeamMemberIndex];
                dropPosition = _sceneConfig.RightDropBallPoint.transform;
            }
            else
            {
                droppingPlayer = _leftTeam[NumericConstants.PrimaryTeamMemberIndex];
                dropPosition = _sceneConfig.LeftDropBallPoint.transform;
            }

            droppingPlayer.EnterNotControlledState();
            droppingPlayer.transform.CopyValuesFrom(dropPosition, false);
            _ball.SetOwner(droppingPlayer);
        }

        private void EnterGameplayState() => _gameplayLoopStateMachine.Enter<GameplayState>();
    }
}