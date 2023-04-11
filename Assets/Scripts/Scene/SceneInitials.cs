using System.Linq;
using Gameplay;
using UnityEngine;
using Utility.Constants;

namespace Scene
{
    public class SceneInitials : MonoBehaviour
    {
        [SerializeField] private Ring.Ring _rightRing;
        [SerializeField] private Ring.Ring _leftRing;
        [SerializeField] private Transform _courtCenter;
        [SerializeField] private Transform _leftDownCorner;
        [SerializeField] private Transform _rightUpCorner;
        [SerializeField] private Transform[] _leftTeamDropBallPositions = new Transform[NumericConstants.PlayersInTeam];
        [SerializeField] private Transform[] _rightTeamDropBallPositions = new Transform[NumericConstants.PlayersInTeam];

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
        private Vector3 LeftDownCorner => _leftDownCorner.position;
        private Vector3 RightUpCorner => _rightUpCorner.position;
        private Vector3 CenterPosition => _courtCenter.position;

        public Transform[] LeftTeamDropBallPositions => _leftTeamDropBallPositions.ToArray();

        public Transform[] RightTeamDropBallPositions => _rightTeamDropBallPositions.ToArray();
    }
}