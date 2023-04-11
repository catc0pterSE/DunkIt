using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Pass
{
    public class Catcher: SwitchableMonoBehaviour
    {
        [SerializeField] private CharacterFacade _host;
        
        public event Action CaughtBall;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Ball.MonoBehavior.Ball ball) == false)
                return;
            
            ball.SetOwner(_host);
            CaughtBall?.Invoke();
        }
    }
}