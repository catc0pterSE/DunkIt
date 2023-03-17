using System;
using System.Collections;
using Gameplay.Character;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Ball.MonoBehavior
{
    public class Ball : SwitchableMonoBehaviour
    {
        [SerializeField] private float _transferSpeed = 2f;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _lostTimeout = 5f;

        private CharacterFacade _owner;
        private Coroutine _moveRoutine;
        private Coroutine _trackLostRoutine;
        private WaitForSeconds _waitForLostTimeout;

        private void Awake() =>
            _waitForLostTimeout = new WaitForSeconds(_lostTimeout);

        public CharacterFacade Owner => _owner;

        public event Action<CharacterFacade> OwnerChanged;
        public event Action<CharacterFacade> Lost;

        public float Mass => _rigidBody.mass;

        public void SetOwner(CharacterFacade owner)
        {
            StopTrackingLost();
            TurnPhysicsOf();
            RemoveOwner();
            transform.Reset(false);
            SetParent(owner.BallPosition);
            _owner = owner;
            OwnerChanged?.Invoke(_owner);
        }

        public void Fly(Vector3 velocity)
        {
            CharacterFacade lasrOwner = _owner;
            StartTrackingLost(lasrOwner);
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

        private void StartTrackingLost(CharacterFacade lastOwner)
        {
            StopTrackingLost();
            _trackLostRoutine = StartCoroutine(TrackLost(lastOwner));
        }

        private void StopTrackingLost()
        {
            if (_trackLostRoutine != null)
                StopCoroutine(_trackLostRoutine);
        }

        public void MoveTo(Vector3 targetPosition, Action toDoAfter = null)
        {
            RemoveOwner();
            TurnPhysicsOf();

            if (_moveRoutine != null)
                StopCoroutine(_moveRoutine);

            _moveRoutine = StartCoroutine(Move(targetPosition, toDoAfter));
        }

        public void MoveTo(Transform target, Action toDoAfter = null) =>
            MoveTo(target.position, toDoAfter);

        public void SetOwnerSmoothly(CharacterFacade newOwner, Action toDoAfter = null) =>
            MoveTo(newOwner.BallPosition, () =>
            {
                SetOwner(newOwner);
                toDoAfter?.Invoke();
            });

        private IEnumerator Move(Vector3 targetPosition, Action callback = null)
        {
            while (transform.position != targetPosition)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPosition, _transferSpeed * Time.deltaTime);
                yield return null;
            }

            callback?.Invoke();
        }

        private IEnumerator TrackLost(CharacterFacade lastOwner)
        {
            yield return _waitForLostTimeout;

            if (_owner == null)
                Lost?.Invoke(lastOwner);
        }

        private void SetParent(Transform parent) =>
            transform.SetParent(parent, false);

        private void TurnPhysicsOn() =>
            _rigidBody.isKinematic = false;

        private void TurnPhysicsOf() =>
            _rigidBody.isKinematic = true;
    }
}