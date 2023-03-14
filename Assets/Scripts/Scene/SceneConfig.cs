using UnityEngine;

namespace Scene
{
    public class SceneConfig : MonoBehaviour
    {
        [SerializeField] private Ring.Ring _rightRing;
        [SerializeField] private Ring.Ring _leftRing;
        [SerializeField] private Transform _enemyDropBallPoint;
        [SerializeField] private Transform _playerDropBallPoint;
        [SerializeField] private Transform _courtCenter;
        [SerializeField] private Transform _leftDownCorner;
        [SerializeField] private Transform _rightUpCorner;

        public CourtDimensions CourtDimensions => new CourtDimensions
        (
            CenterPosition,
            LeftDownCorner.x,
            RightUpCorner.x,
            LeftDownCorner.z,
            RightUpCorner.z
        );

        public Ring.Ring RightRing => _rightRing;
        public Ring.Ring LeftRing => _leftRing;
        public Transform EnemyDropBallPoint => _enemyDropBallPoint;
        public Transform PlayerDropBallPoint => _playerDropBallPoint;

        private Vector3 LeftDownCorner => _leftDownCorner.position;
        private Vector3 RightUpCorner => _rightUpCorner.position;
        private Vector3 CenterPosition => _courtCenter.position;
    }
}