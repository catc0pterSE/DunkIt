using System.ComponentModel;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw;
using Gameplay.Character.Player.MonoBehaviour.Brains;
using NodeCanvas.Framework;
using Scene.Ring;
using UnityEngine;
using Utility.Constants;

namespace NC_Custom_Tasks.Actions
{
    public class Throw : ActionTask
    {
        [BlackboardOnly] public BBParameter<Ring> OppositeRing;
        [BlackboardOnly] public BBParameter<AIControlledEventLauncher> AIControlledEventLauncher;
        [BlackboardOnly] public BBParameter<BallThrower> BallThrower;
        [BlackboardOnly] public BBParameter<Transform> BallHolder;
        public float MaxBallSpeed;
        public float MinAimDeviation;
        public float MaxAimDeviation;

        protected override void OnExecute()
        {
            AIControlledEventLauncher.value.InitiateThrow();
            BallThrower.value.Throw(CalculateVelocity());
            EndAction(true);
        }

        private Vector3 CalculateVelocity()
        {
            Vector3 toTarget = GetTarget() - BallHolder.value.transform.position;
            float gSquared = Physics.gravity.sqrMagnitude;
            float potentialEnergy = MaxBallSpeed * MaxBallSpeed + Vector3.Dot(toTarget, Physics.gravity);
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
            Vector3 ringCenter = OppositeRing.value.RingCenter;
            Vector3 randomInSphere = Random.insideUnitSphere * Random.Range(MinAimDeviation, MaxAimDeviation);
            return ringCenter + randomInSphere;
        }
    }
}