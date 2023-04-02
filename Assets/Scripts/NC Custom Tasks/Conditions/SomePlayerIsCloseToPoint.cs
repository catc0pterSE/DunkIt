using Gameplay.Character.Player.MonoBehaviour;
using NodeCanvas.Framework;
using UnityEngine;

namespace NC_Custom_Tasks.Conditions
{
    public class SomePlayerIsCloseToPoint: ConditionTask
    {
        [BlackboardOnly] public BBParameter<PlayerFacade> Player;
        [BlackboardOnly] public BBParameter<Vector3> Point;
        public float Delta;
        protected override bool OnCheck()
        {
            Vector3 playerPosition = Player.value.transform.position;
            Vector3 projectedPoint = new Vector3(Point.value.x, playerPosition.y, Point.value.z);
            return Vector3.Distance(playerPosition, projectedPoint) <= Delta;
        }
    }
}