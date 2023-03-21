using NodeCanvas.Framework;
using Utility.Constants;

namespace NC_Custom_Tasks.Actions
{
    public class IncreaseFloat : ActionTask
    {
        [BlackboardOnly] public BBParameter<float> FloatToIncrease;
        public float Percents;

        protected override void OnExecute()
        {
            FloatToIncrease.value += FloatToIncrease.value*Percents/NumericConstants.MaxPercents;
            EndAction(true);
        }
    }
}