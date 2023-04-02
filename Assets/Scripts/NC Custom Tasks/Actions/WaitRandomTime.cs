using System.Collections;
using NodeCanvas.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NC_Custom_Tasks.Actions
{
    public class WaitRandomTime : ActionTask
    {
        public float Min;
        public float Max;

        private Coroutine waitRoutine;

        protected override void OnExecute()
        {
            if (waitRoutine!=null)
                StopCoroutine(waitRoutine);

            waitRoutine = StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(Random.Range(Min,Max));
            EndAction(true);
        }
    }
}