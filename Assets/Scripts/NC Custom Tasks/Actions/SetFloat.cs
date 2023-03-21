using NodeCanvas.Framework;

namespace NC_Custom_Tasks.Actions
{
    public class SetFloat: ActionTask
    {
        [BlackboardOnly] public BBParameter<float> FloatToSet;
        public float Value;

        protected override void OnExecute()
        {
            FloatToSet.value = Value;
            EndAction(true);
        }
    }
}