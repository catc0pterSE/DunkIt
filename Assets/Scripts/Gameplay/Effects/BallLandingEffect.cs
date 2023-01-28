using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Effects
{
    public class BallLandingEffect: SwitchableMonoBehaviour
    {
        public void Settle(RaycastHit hitPoint)
        {
            Transform effectTransform = transform;
            
            effectTransform.position = hitPoint.point;
            effectTransform.forward = hitPoint.normal;
        }
    }
}