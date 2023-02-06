using System;
using System.Collections;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using Modules.MonoBehaviour;
using Scene.Ring;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;
using Random = UnityEngine.Random;

namespace Gameplay.Character.Player.MonoBehaviour.BallHandle.Dunk
{
    using Ball.MonoBehavior;
    public class Dunker : SwitchableComponent
    {
        [SerializeField] private PlayerMover _mover;
        [SerializeField] private float _jumpSpeed = 6;

        private Ball _ball;

        private Coroutine _jumping;

        public event Action DunkPointReached;

        public void Dunk(Ring ring)
        {
            _mover.RotateTo(ring.transform.position);
            

            if (_jumping != null)
                StopCoroutine(_jumping);

            _jumping = StartCoroutine(JumpThenThrow(ring));
        }

        private IEnumerator JumpThenThrow(Ring ring)
        {
            Vector3 jumpTargetPosition = ring.DunkPoints.FindClosest(transform.position).position;
            
            while (transform.position != jumpTargetPosition)
            {
                Vector3 currentPosition = transform.position;
                transform.position =
                    Vector3.MoveTowards(currentPosition, jumpTargetPosition, _jumpSpeed * Time.deltaTime);
                yield return null;
            }

            DunkPointReached?.Invoke();
            
            _ball.RemoveOwner();
            _ball.transform.position = ring.BallDunkTarget.position;
            Vector3 throwDirection = new Vector3(Vector3.down.x + Random.Range(-NumericConstants.Half, NumericConstants.Half), Vector3.down.y,
                Vector3.down.z + Random.Range(-NumericConstants.Half, NumericConstants.Half)) * NumericConstants.DunkThrowForce;
            _ball.Throw(throwDirection);
        }
    }
}