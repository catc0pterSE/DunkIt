using UnityEngine;

namespace Scene
{
    public class SceneConfig : MonoBehaviour
    {
        [SerializeField] private Ring.Ring _playerRing;
        [SerializeField] private Ring.Ring _enemyRing;
        
        public Ring.Ring PlayerBasket => _playerRing;
        public Ring.Ring EnemyRing => _enemyRing;
        
        
    }
}