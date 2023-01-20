using Gameplay.Cutscene;
using UnityEngine;

namespace Scene
{
    public class SceneConfig : MonoBehaviour
    {
        [SerializeField] private Transform _playerBucket;
        [SerializeField] private Transform _enemyBascket;
      
        public Transform PlayerBucket => _playerBucket;
        public Transform EnemyBascket => _enemyBascket;

      
    }
}