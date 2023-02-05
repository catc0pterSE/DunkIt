using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.TriggerZone
{
    using Ball.MonoBehavior;
    public class Catcher: SwitchableMonoBehaviour
    {
        [SerializeField] private CharacterFacade _host;

        public event Action CaughtBall;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Ball ball) == false)
                return;
            
            ball.SetOwner(_host);
            CaughtBall?.Invoke();
        }
    }
}