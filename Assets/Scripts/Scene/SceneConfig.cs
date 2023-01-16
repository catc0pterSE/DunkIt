using Gameplay.Cutscene;
using UnityEngine;

namespace Scene
{
    public class SceneConfig : MonoBehaviour
    {
        [SerializeField] private Transform _playerBucket;
        [SerializeField] private Transform _enemyBucket;
      
        public Transform PlayerBucket => _playerBucket;
        public Transform EnemyBucket => _enemyBucket;

      
    }
}