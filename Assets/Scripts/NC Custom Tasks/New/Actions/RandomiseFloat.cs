using System;
using NodeCanvas.Framework;
using Random = UnityEngine.Random;

namespace NC_Custom_Tasks.Actions
{
    public class RandomiseFloat: ActionTask
    {
        [BlackboardOnly] public BBParameter<float> Float;
        public float Min;
        public float Max;

        protected override void OnExecute()
        {
            Float.value = Random.Range(Min, Max);
            EndAction(true);
        }
    }
}