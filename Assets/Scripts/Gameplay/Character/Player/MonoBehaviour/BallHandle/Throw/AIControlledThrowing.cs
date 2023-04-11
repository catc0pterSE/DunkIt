using System.ComponentModel;
using Modules.MonoBehaviour;
using Scene.Ring;
using UnityEngine;
using Utility.Constants;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw
{
    public class AIControlledThrowing: SwitchableComponent
    {
        [SerializeField] private BallThrower _ballThrower;
        [SerializeField] private Transform _ballHolder;
        [SerializeField] private float _minBallSpeed;
        [SerializeField] private float _maxBallSpeed;
        [SerializeField] private float _minAimDeviation;
        [SerializeField] private float _maxAimDeviation;
        
        private Ring _oppositeRing;

        public void Initialize(Ring oppositeRing)
        {
            _oppositeRing = oppositeRing;
        }
        
        private void OnEnable() =>
            _ballThrower.Throw(CalculateVelocity());

        private Vector3 CalculateVelocity()
        {
            Vector3 toTarget = GetTarget() - _ballHolder.transform.position;
            float gSquared = Physics.gravity.sqrMagnitude;
            float potentialEnergy = _maxBallSpeed * _maxBallSpeed + Vector3.Dot(toTarget, Physics.gravity);
            float discriminant = potentialEnergy * potentialEnergy - gSquared * toTarget.sqrMagnitude;

            if (discriminant < 0)
                throw new WarningException(
                    "discriminant is negative in attempt to throw a Ball. Max Ball speed is too low");

            // float discriminantRoot = Mathf.Sqrt(discriminant);
            // float maxFlightTime = Mathf.Sqrt((potentialEnergy + discriminantRoot) * NumericConstants.Double / gSquared);
            // float minFlightTime = Mathf.Sqrt((potentialEnergy - discriminantRoot) * NumericConstants.Double / gSquared);
            float lowEnergyTime = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4 / gSquared));

            // float flightTime = Random.Range(minFlightTime, maxFlightTime);
            float flightTime = lowEnergyTime;

            Vector3 launchVelocity = toTarget / flightTime - Physics.gravity * (flightTime * NumericConstants.Half);


            return launchVelocity;
        }
        
        private Vector3 GetTarget()
        {
            Vector3 ringCenter = _oppositeRing.RingCenter;
            Vector3 randomInSphere = Random.insideUnitSphere * Random.Range(_minAimDeviation, _maxAimDeviation);
            return ringCenter + randomInSphere;
        }
    }
}