using System.ComponentModel;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw;
using Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled;
using NodeCanvas.Framework;
using Scene.Ring;
using UnityEngine;
using Utility.Constants;

namespace NC_Custom_Tasks.Actions
{
    public class Throw : ActionTask
    {
        [BlackboardOnly] public BBParameter<Ring> OppositeRing;
        [BlackboardOnly] public BBParameter<AIControlledBrain> AIControlledEventLauncher;
        [BlackboardOnly] public BBParameter<LocalControlledThrowing> BallThrower;
        [BlackboardOnly] public BBParameter<Transform> BallHolder;
        public float MaxBallSpeed;
        public float MinAimDeviation;
        public float MaxAimDeviation;

        protected override void OnExecute()
        {
            
            EndAction(true);
        }

       

        
    }
}