using System;
using UnityEngine;
using Utility.Constants;

namespace Scene
{
    public class SceneConfig : MonoBehaviour
    {
        [Header("Essentials")] [SerializeField]
        private Transform _playerBucket;

        [SerializeField] private Transform _enemyBucket;

        [Header("Start Cutscene")] [SerializeField]
        private CameraRoutePoint[] _cameraRoute;

        [SerializeField] private Transform[] _playerTeamPositions;
        [SerializeField] private Transform _playerTeamCameraTarget;
        [SerializeField] private Transform[] _enemyTeamPositions;
        [SerializeField] private Transform _enemyTeamCameraTarget;


        [Header("Referee ball")] [SerializeField]
        private Transform _refereePosition;

        [SerializeField] private Transform[] _playerRoute;
        [SerializeField] private Transform[] _enemyRoute;

        [Header("Dunk")] [SerializeField] private Transform[] _cameraPositions;

        public Transform PlayerBucket => _playerBucket;

        public Transform EnemyBucket => _enemyBucket;

        public Transform PlayerTeamCameraTarget => _playerTeamCameraTarget;

        public Transform EnemyTeamCameraTarget => _enemyTeamCameraTarget;

        public CameraRoutePoint[] CameraRoute => _cameraRoute;

        public Transform[] PlayerTeamPositions => _playerTeamPositions;

        public Transform[] EnemyTeamPositions => _enemyTeamPositions;

        public Transform RefereePosition => _refereePosition;

        public Transform[] PlayerRoute => _playerRoute;

        public Transform[] EnemyRoute => _enemyRoute;

        public Transform[] CameraPositions => _cameraPositions;


        private void OnValidate()
        {
            if (_playerTeamPositions == null)
                _playerTeamPositions = new Transform[NumericConstants.PlayersInTeam];

            if (_enemyTeamPositions == null)
                _enemyTeamPositions = new Transform[NumericConstants.PlayersInTeam];
        }
    }
}