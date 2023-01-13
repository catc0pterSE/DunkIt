using UnityEngine;

namespace Gameplay.Cutscene
{
    public class CutsceneConfig : MonoBehaviour
    {
        [SerializeField] private Transform[] _playerTeamStartPositions;
        [SerializeField] private Transform[] _enemyTeamStartPositions;
        [SerializeField] private Transform _refereeStartPosition;
        [SerializeField] private Transform _cameraStartPosition;
        [SerializeField] private Transform[] _player1Route;
        [SerializeField] private Transform[] _player2Route;
        [SerializeField] private Transform[] _enemy1Route;
        [SerializeField] private Transform[] _enemy2Route;
        [SerializeField] private Transform[] _refereeRoute;

        private CameraRoutePoint[] _cameraRoute;

        public Transform[] PlayerTeamStartPositions => _playerTeamStartPositions;

        public Transform[] EnemyTeamStartPositions => _enemyTeamStartPositions;

        public Transform CameraStartPosition => _cameraStartPosition;

        public Transform RefereeStartPosition => _refereeStartPosition;

        public Transform[] Player1Route => _player1Route;

        public Transform[] Player2Route => _player2Route;

        public Transform[] Enemy1Route => _enemy1Route;

        public Transform[] Enemy2Route => _enemy2Route;

        public Transform[] RefereeRoute => _refereeRoute;

        public CameraRoutePoint[] CameraRoute => _cameraRoute ??= GetComponentsInChildren<CameraRoutePoint>();
    }
}