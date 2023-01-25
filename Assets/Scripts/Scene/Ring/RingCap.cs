using System;
using Gameplay.Ball.MonoBehavior;
using UnityEngine;

namespace Scene.Ring
{
    public class RingCap : MonoBehaviour
    {
        [SerializeField] private Collider _collider;

        public event Action Pierced;
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Ball>(out _))
                Unlock();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<Ball>(out _))
            {
                Lock();
                Pierced?.Invoke();
            }
        }
        
        private void Unlock()=>
            _collider.isTrigger = true;
        
        private void Lock()=>
            _collider.isTrigger = false;
    }
}