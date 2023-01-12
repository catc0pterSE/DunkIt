using System;
using Gameplay.Camera.MonoBehaviour;
using Gameplay.Camera.StateMachine.States;
using Gameplay.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.NPC.Referee.MonoBehaviour;
using Gameplay.Player.MonoBehaviour;
using Modules.StateMachine;
using Scene;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    using Ball.MonoBehavior;
    public class StartCutsceneState : IParameterlessState
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly EnemyFacade[] _enemyTeam;
        private readonly Referee _referee;
        private readonly Ball _ball;
        private readonly CameraFacade _camera;
        private readonly SceneConfig _sceneConfig;

        public StartCutsceneState(PlayerFacade[] playerTeam, EnemyFacade[] enemyTeam, Referee referee, Ball ball, CameraFacade camera, SceneConfig sceneConfig)
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _referee = referee;
            _ball = ball;
            _camera = camera;
            _sceneConfig = sceneConfig;
        }

        public void Enter()
        {
            ArrangeCharacters();
            SetBallPosition();
            SetCharactersStates();
            
            _camera.StateMachine.Enter<DynamicCutsceneState, CameraRoutePoint[]>(_sceneConfig.CameraRoute);
            _camera.RouteFollower.Finished += OnCameraMovementFinished;
        }
        
        public void Exit()
        {
            
        }

        private void OnCameraMovementFinished()
        {
           
        }

        private void ArrangeCharacters()
        {
            ArrangeTransformArray(_playerTeam.GetTransforms(), _sceneConfig.PlayerTeamPositions);
            ArrangeTransformArray(_enemyTeam.GetTransforms(), _sceneConfig.EnemyTeamPositions);
            _referee.transform.CopyValuesFrom(_sceneConfig.RefereePosition);
        }

        private void SetBallPosition()
        {
           _ball.SetParent(_referee.BallPosition);
        }

        private void ArrangeTransformArray(Transform[] transforms, Transform[] points)
        {
            if (transforms.Length != points.Length)
                throw new Exception("Lengths of arrays are unequal");

            for (int i = 0; i < transforms.Length; i++)
            {
                transforms[i].CopyValuesFrom(points[i]);
            }
        }

        private void SetCharactersStates()
        {
            _playerTeam.Map(player => player.StateMachine.Enter<Player.StateMachine.States.CutsceneState>());
            _enemyTeam.Map(enemy => enemy.StateMachine.Enter<NPC.EnemyPlayer.StateMachine.States.CutsceneState>());
        }
    }
}