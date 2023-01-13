using Gameplay.Cutscene;
using UnityEngine;

namespace Scene
{
    public class SceneConfig : MonoBehaviour
    {
        [Header("Essentials")] [SerializeField]
        private Transform _playerBucket;

        [SerializeField] private Transform _enemyBucket;

        [Header("Start Cutscene")] [SerializeField]
        private CutsceneConfig _startCutsceneConfig;

        [Header("Referee ball")] [SerializeField]
        private Transform _refereePosition;

        [SerializeField] private Transform[] _playerRoute;
        [SerializeField] private Transform[] _enemyRoute;

        [Header("Dunk")] [SerializeField] private Transform[] _cameraPositions;

        public Transform PlayerBucket => _playerBucket;

        public Transform EnemyBucket => _enemyBucket;
        
        public CutsceneConfig StartCutsceneConfig => _startCutsceneConfig;
        public Transform RefereePosition => _refereePosition;

        public Transform[] PlayerRoute => _playerRoute;

        public Transform[] EnemyRoute => _enemyRoute;

        public Transform[] CameraPositions => _cameraPositions;
    }
}