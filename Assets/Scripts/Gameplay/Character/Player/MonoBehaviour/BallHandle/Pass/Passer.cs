using System;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Constants;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Pass
{
    public class Passer : SwitchableComponent
    {
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private float _flightTimeCoefficient = 0.5f; // TODO: depend on distance?

        private Ball.MonoBehavior.Ball _ball;

        public void Initialize(Ball.MonoBehavior.Ball ball) =>
            _ball = ball;

        public event Action PassedBall;

        public void Pass(PlayerFacade to)
        {
            _ball.Fly(CalculateVelocity(to.transform.position));
            PassedBall?.Invoke();
        }

        private Vector3 CalculateVelocity(Vector3 target)
        {
            Vector3 toTarget = target - _ballPosition.position;
            float gSquared = Physics.gravity.sqrMagnitude;
            float flightTime =
                Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * NumericConstants.LowEnergyParabolicCoefficient / gSquared)) *
                _flightTimeCoefficient;
            Vector3 velocity = toTarget / flightTime - Physics.gravity * (flightTime * NumericConstants.Half);
            return velocity;
        }
    }
}