using System;
using Gameplay.Ball.MonoBehavior;
using UnityEngine;

namespace Scene.Ring
{
    public class WinZone: MonoBehaviour
    {
        public event Action Entered; 
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<Ball>(out _))
                Entered?.Invoke();
        }
    }
}