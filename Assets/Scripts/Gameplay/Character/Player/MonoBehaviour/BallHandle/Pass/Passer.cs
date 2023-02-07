using System;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Constants;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Pass
{
    using Ball.MonoBehavior;
    public class Passer: SwitchableComponent
    {
        [SerializeField] private Transform _ballPosition;
        
        private BasketballPlayerFacade _ally;
        private Ball _ball;

        public event Action PassedBall;
        
        public void Initialize(Ball ball, PlayerFacade ally)
        {
            _ally = ally;
            _ball = ball;
        }
        
        public void Pass()
        {
          _ball.Throw(CalculateVelocity());
          PassedBall?.Invoke();
        }

        private Vector3 CalculateVelocity()
        {
            Vector3 toTarget = _ally.BallPosition.position  - _ballPosition.position;
            float gSquared = Physics.gravity.sqrMagnitude;
            float flightTime = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * NumericConstants.LowEnergyParabolicFlightTimeCoefficient / gSquared))*NumericConstants.Half;
            Vector3 velocity = toTarget / flightTime - Physics.gravity * (flightTime * NumericConstants.Half);
            return velocity;
        }
    }
}