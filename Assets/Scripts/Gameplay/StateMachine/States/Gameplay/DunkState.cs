using Cinemachine;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.StateMachine.States;
using Gameplay.StateMachine.States.CutsceneStates;
using Modules.StateMachine;
using Scene;
using Scene.Ring;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.Gameplay
{
    using Ball.MonoBehavior;

    public class DunkState : IParameterState<PlayerFacade>
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly EnemyFacade[] _enemyTeam;
        private readonly Ball _ball;
        private readonly SceneConfig _sceneConfig;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        private CinemachineVirtualCamera _dunkVirtualCamera;

        private PlayerFacade _jumpingPlayer;

        public DunkState(PlayerFacade[] playerTeam, EnemyFacade[] enemyTeam, Ball ball, SceneConfig sceneConfig,
            GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _ball = ball;
            _sceneConfig = sceneConfig;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enter(PlayerFacade player)
        {
            _jumpingPlayer = player;
            SetUpDunkCamera();
            SetCharactersStates();
            SubscribeOnJumpingPlayer();
        }

        public void Exit()
        {
            UnsubscribeFromJumpingPlayer();
            UnsubscribeFromGoalScored();
        }

        private void SetUpDunkCamera()
        {
            _dunkVirtualCamera = _sceneConfig.EnemyRing.DunkVirtualCamera;
            _dunkVirtualCamera.Prioritize();
            _dunkVirtualCamera.LookAt = _ball.transform;
        }

        private void SubscribeOnJumpingPlayer() =>
            _jumpingPlayer.DunkPointReached += OnDunkPointReached;

        private void UnsubscribeFromJumpingPlayer() =>
            _jumpingPlayer.DunkPointReached -= OnDunkPointReached;

        private void SubscribeOnGoalScored() =>
            _sceneConfig.EnemyRing.Goal += MoveToCelebrateCutscene;

        private void UnsubscribeFromGoalScored() =>
            _sceneConfig.EnemyRing.Goal += MoveToCelebrateCutscene;

        private void MoveToCelebrateCutscene()
        {
            _gameplayLoopStateMachine.Enter<CelebrateCutsceneState>();
        }

        private void OnDunkPointReached()
        {
            SubscribeOnGoalScored();
        }

        private void SetCharactersStates()
        {
            PlayerFacade otherPlayer = _playerTeam.FindFirstOrNull(player => player != _jumpingPlayer);
            otherPlayer.EnterIdleState();
            _enemyTeam.Map(enemy => enemy.EnterIdleState());
            _jumpingPlayer.EnterDunkState(_sceneConfig.EnemyRing);
        }
    }
}