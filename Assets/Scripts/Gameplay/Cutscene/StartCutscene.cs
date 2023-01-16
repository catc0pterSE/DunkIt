using System;
using System.Collections;
using Cinemachine;
using Gameplay.Camera.MonoBehaviour;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Infrastructure.CoroutineRunner;
using Infrastructure.Factory;
using Infrastructure.ServiceManagement;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Cutscene
{
    using Ball.MonoBehavior;

    public class StartCutscene : ICutscene
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly EnemyFacade[] _enemyTeam;
        private readonly Ball _ball;
        private readonly Referee _referee;
        private readonly CameraFacade _camera;
        private readonly CutsceneConfig _cutsceneConfig;
        private readonly ICoroutineRunner _coroutineRunner;
        
        private  MultipleObjectFollower _playerTeamFollower;
        private  MultipleObjectFollower _enemyTeamFollower;

        private Coroutine _running;

        public event Action Finished;

        public StartCutscene(
            PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam,
            Ball ball,
            Referee referee,
            CameraFacade camera,
            CutsceneConfig cutsceneConfig,
            ICoroutineRunner coroutineRunner
        )
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _ball = ball;
            _referee = referee;
            _camera = camera;
            _cutsceneConfig = cutsceneConfig;
            _coroutineRunner = coroutineRunner;
        }

        private void SpawnTeamFollowers()
        {
            _playerTeamFollower = SpawnTeamFollower(_playerTeam.GetTransforms());
            _enemyTeamFollower = SpawnTeamFollower(_enemyTeam.GetTransforms());
        }
        
        public void Run()
        {
            ArrangeCharacters();
            SpawnTeamFollowers();
            SetBallPosition();
            SetCameraStartPosition();
            InitializeCameraRoute();

           if (_running != null)
                _coroutineRunner.StopCoroutine(_running);

            _running = _coroutineRunner.StartCoroutine(Act());
        }

        private void InitializeCameraRoute()
        {
            _cutsceneConfig.CameraRoute[0].SetFocusTarget(_playerTeamFollower.transform);
            _cutsceneConfig.CameraRoute[2].SetFocusTarget(_enemyTeamFollower.transform);
            _cutsceneConfig.CameraRoute[4].SetFocusTarget(_referee.transform);
        }

        private MultipleObjectFollower SpawnTeamFollower(Transform[] team)
        {
            MultipleObjectFollower teamFollower =
                Services.Container.Single<IGameObjectFactory>().CreateMultipleObjectFollower(team.GetTransformPositions().GetIntermediatePosition());
            teamFollower.Initialize(team);
            return teamFollower;
        }

        private void SetCameraStartPosition()
        {
            _camera.transform.CopyValuesFrom(_cutsceneConfig.CameraStartPosition);
            _camera.SetFocusTarget(_playerTeamFollower.transform, true);
        }

        private void ArrangeCharacters()
        {
            ArrangeTransformArray(_playerTeam.GetTransforms(), _cutsceneConfig.PlayerTeamStartPositions);
            ArrangeTransformArray(_enemyTeam.GetTransforms(), _cutsceneConfig.EnemyTeamStartPositions);
            _referee.transform.CopyValuesFrom(_cutsceneConfig.RefereeStartPosition);
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

        private void SetBallPosition()
        {
            _ball.SetParent(_referee.BallPosition);
        }

        private IEnumerator Act()
        {
            CameraRoutePoint[] route = _cutsceneConfig.CameraRoute;
            Transform cameraTransform = _camera.transform;

            for (int i = 0; i < route.Length; i++)
            {
                if (route[i].FocusTarget !=null)
                    _camera.SetFocusTarget(route[i].FocusTarget);
                
                float cameraMovementSpeed = route[i].MovementSpeed;
                Vector3 pointPosition = route[i].transform.position;
                
                while (cameraTransform.position != pointPosition)
                {
                    cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, pointPosition,
                        cameraMovementSpeed * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }
}