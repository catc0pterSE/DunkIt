using System.Collections;
using System.ComponentModel;
using Modules.MonoBehaviour;
using Scene.Ring;
using UnityEngine;
using Utility.Constants;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw
{
    public class AIControlledThrowing : SwitchableComponent
    {
        [SerializeField] private BallThrower _ballThrower;
        [SerializeField] private Transform _ballHolder;
        [SerializeField] private float _maxBallSpeed;
        [SerializeField] private float _minAimDeviation;
        [SerializeField] private float _maxAimDeviation;
        [SerializeField] private float _minAimingTime = 0.5f;
        [SerializeField] private float _maxAimingTime = 1.5f;

        private Ring _oppositeRing;
        private Coroutine _aimAndThrowRoutine;

        public void Initialize(Ring oppositeRing)
        {
            _oppositeRing = oppositeRing;
        }

        private void OnEnable()
        {
            StopAimAndThrowRoutine();
            _aimAndThrowRoutine = StartCoroutine(AimAndThrow());
        }

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

        private void StopAimAndThrowRoutine()
        {
            if (_aimAndThrowRoutine != null)
                StopCoroutine(_aimAndThrowRoutine);
        }

        private IEnumerator AimAndThrow()
        {
            yield return new WaitForSeconds(Random.Range(_minAimingTime, _maxAimingTime));
            _ballThrower.Throw(CalculateVelocity());
        }
    }
}