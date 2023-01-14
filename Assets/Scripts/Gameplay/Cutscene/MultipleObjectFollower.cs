using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Cutscene
{
    public class MultipleObjectFollower : MonoBehaviour
    {
        private Transform[] _targets;

        private void LateUpdate()
        {
            if (_targets != null)
                transform.position = _targets.GetTransformPositions().GetIntermediatePosition();
        }

        public void Initialize(Transform[] targets)
        {
            _targets = targets;
        }
    }
}