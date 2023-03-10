using System;
using System.Collections;
using Gameplay.Character;
using Gameplay.Character.Player.MonoBehaviour;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Ball.MonoBehavior
{
    public class Ball : SwitchableMonoBehaviour
    {
        [SerializeField] private float _transferSpeed = 2f;
        [SerializeField] private Rigidbody _rigidBody;

        private CharacterFacade _owner;
        private Coroutine _moving;

        public CharacterFacade Owner => _owner;

        public event Action<CharacterFacade> OwnerChanged;

        public float Mass => _rigidBody.mass;

        public void SetOwner(CharacterFacade owner)
        {
            TurnPhysicsOf();
            RemoveOwner();
            transform.Reset(false);
            SetParent(owner.BallPosition);
            _owner = owner;
            OwnerChanged?.Invoke(_owner);
        }

        public void Fly(Vector3 velocity)
        {
            RemoveOwner();
            TurnPhysicsOn();
            _rigidBody.AddForce(velocity, ForceMode.VelocityChange);
        }

        private void RemoveOwner()
        {
            transform.parent = null;
            _owner = null;
            OwnerChanged?.Invoke(_owner);
        }

        public void MoveTo(Vector3 targetPosition, Action toDoAfter = null)
        {
            RemoveOwner();
            TurnPhysicsOf();

            if (_moving != null)
                StopCoroutine(_moving);

            _moving = StartCoroutine(MoveRoutine(targetPosition, toDoAfter));
        }

        public void MoveTo(Transform target, Action toDoAfter = null) =>
            MoveTo(target.position, toDoAfter);

        public void SetOwnerSmoothly(CharacterFacade newOwner, Action toDoAfter = null) =>
            MoveTo(newOwner.BallPosition, () =>
            {
                SetOwner(newOwner);
                toDoAfter?.Invoke();
            });

        private IEnumerator MoveRoutine(Vector3 targetPosition, Action callback = null)
        {
            while (transform.position != targetPosition)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPosition, _transferSpeed * Time.deltaTime);
                yield return null;
            }

            callback?.Invoke();
        }

        private void SetParent(Transform parent) =>
            transform.SetParent(parent, false);

        private void TurnPhysicsOn() =>
            _rigidBody.isKinematic = false;

        private void TurnPhysicsOf() =>
            _rigidBody.isKinematic = true;
    }
}