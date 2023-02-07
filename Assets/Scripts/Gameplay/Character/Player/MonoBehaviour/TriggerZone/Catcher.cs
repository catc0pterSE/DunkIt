using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.TriggerZone
{
    using Ball.MonoBehavior;
    public class Catcher: SwitchableMonoBehaviour
    {
        [SerializeField] private CharacterFacade _host;

        private Ball _ball;
        
        public event Action CaughtBall;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Ball ball) == false)
                return;
            _ball = ball;
            

            void GetBall()
            {
                _ball.SetOwner(_host);
                CaughtBall?.Invoke();
            }
            
            _ball.MoveTo(_host.BallPosition.position, GetBall);
        }
    }
}