using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle
{
    public class BallThrower: SwitchableComponent
    {
        private Ball.MonoBehavior.Ball _ball;

        public void Initialize(Ball.MonoBehavior.Ball ball)
        {
            _ball = ball;
        }
        
        public event Action BallThrown;
        
        public void Throw(Vector3 launchVelocity)
        {
            _ball.Fly(launchVelocity);
            BallThrown?.Invoke();
            Disable();
        }
    }
}