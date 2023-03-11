using UnityEngine;

namespace Scene
{
    public class SceneConfig : MonoBehaviour
    {
        [SerializeField] private Ring.Ring _playerRing;
        [SerializeField] private Ring.Ring _enemyRing;
        [SerializeField] private Transform _enemyDropBallPoint;
        [SerializeField] private Transform _playerDropBallPoint;
        [SerializeField] private Transform _courtCenter;

        public Transform CourtCenter => _courtCenter;
        public Ring.Ring PlayerRing => _playerRing;
        public Ring.Ring EnemyRing => _enemyRing;

        public Transform EnemyDropBallPoint => _enemyDropBallPoint;

        public Transform PlayerDropBallPoint => _playerDropBallPoint;
    }
}