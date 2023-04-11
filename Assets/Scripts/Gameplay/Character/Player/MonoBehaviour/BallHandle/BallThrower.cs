
using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw
{
    using Ball.MonoBehavior;
    
    public class BallThrower: SwitchableComponent
    {
        private Ball _ball;

        public void Initialize(Ball ball)
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