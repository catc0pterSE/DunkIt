using NodeCanvas.Framework;
using Utility.Constants;

namespace NC_Custom_Tasks.Actions
{
    public class DecreaseFloat : ActionTask
    {
        [BlackboardOnly] public BBParameter<float> FloatToDecrease;
        public float Percents;

        protected override void OnExecute()
        {
            FloatToDecrease.value -= FloatToDecrease.value * Percents / NumericConstants.MaxPercents;
            EndAction(true);
        }
    }
}