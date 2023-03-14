using UnityEngine;

namespace Scene
{
    public class SceneConfig : MonoBehaviour
    {
        [SerializeField] private Ring.Ring _rightRing;
        [SerializeField] private Ring.Ring _leftRing;
        [SerializeField] private Transform _rightDropBallPoint;
        [SerializeField] private Transform _leftDropBallPoint;
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
        public Transform RightDropBallPoint => _rightDropBallPoint;
        public Transform LeftDropBallPoint => _leftDropBallPoint;

        private Vector3 LeftDownCorner => _leftDownCorner.position;
        private Vector3 RightUpCorner => _rightUpCorner.position;
        private Vector3 CenterPosition => _courtCenter.position;
    }
}