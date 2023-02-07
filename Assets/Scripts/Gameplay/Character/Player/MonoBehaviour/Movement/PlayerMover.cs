using System;
using System.Collections;
using Modules.MonoBehaviour;
using Scene.Ring;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour.Movement
{
    public class PlayerMover : SwitchableComponent
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _movementSpeed = 4;
        [SerializeField] private float _rotationSpeed = 400;
        [SerializeField] private float _gravityModifier = 1;

        private Coroutine _rotating;
        
        public void Move(Vector3 movementDirection)
        {
            Rotate(movementDirection);
            movementDirection += GetGravity();

            _characterController.Move(movementDirection * (Time.deltaTime * _movementSpeed));
        }

        public void RotateTo(Vector3 position, Action callback = null)
        {
            Vector3 positionProjection = new Vector3(position.x, transform.position.y, position.z);
            
            if (_rotating!=null)
                StopCoroutine(_rotating);

            _rotating = StartCoroutine(RotateToPosition(positionProjection, callback));
        }
        
       
        private Vector3 GetGravity()
        {
            return Physics.gravity * _gravityModifier;
        }

       private void Rotate(Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }

        private IEnumerator RotateToPosition(Vector3 position, Action callback = null)
        {
            Vector3 direction = position - transform.position;

            while (Vector3.Angle(transform.forward, direction) > NumericConstants.MinimalDelta)
            {
                Rotate(direction);
                yield return null;
            }
            
            callback?.Invoke();
        }
    }
}