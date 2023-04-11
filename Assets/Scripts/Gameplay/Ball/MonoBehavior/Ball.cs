using System;
using System.Collections;
using Gameplay.Character;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw;
using Modules.MonoBehaviour;
using Scene;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Ball.MonoBehavior
{
    public class Ball : SwitchableMonoBehaviour
    {
        [SerializeField] private float _transferSpeed = 2f;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _freeTimeoutSeconds;

        private CharacterFacade _owner;
        private Coroutine _moveRoutine;
        private Coroutine _trackLostRoutine;
        private Coroutine _trackFreeRoutine;
        private CharacterFacade _previousOwner;
        private CourtDimensions _courtDimensions;

        public CharacterFacade Owner => _owner;

        public void Initialize(SceneInitials sceneInitials) =>
            _courtDimensions = sceneInitials.CourtDimensions;

        public event Action<CharacterFacade> OwnerChanged;
        public event Action<CharacterFacade> Lost;
        public event Action Free;

        public void SetOwner(CharacterFacade owner)
        {
            StopTrackFree();
            StopTrackLost();
            RemoveOwner();
            TurnPhysicsOf();
            transform.Reset(false);
            SetParent(owner.BallPosition);
            _owner = owner;
            OwnerChanged?.Invoke(_owner);
        }

        public void Fly(Vector3 velocity)
        {
            RemoveOwner();
            TurnPhysicsOn();
            StartTrackFree();
            StartTrackingLost();
            StartTrackFree();
            StartTrackingLost();

            _rigidBody.AddForce(velocity, ForceMode.VelocityChange);
        }

        private void StartTrackingLost()
        {
            StopTrackLost();
            _trackLostRoutine = StartCoroutine(TrackLost());
        }

        private void StopTrackLost()
        {
            if (_trackLostRoutine != null)
                StopCoroutine(_trackLostRoutine);
        }

        public void StartMovingTo(Vector3 targetPosition, Action toDoAfter = null)
        {
            TurnPhysicsOf();
            StopMovingRoutine();
            _moveRoutine = StartCoroutine(MoveTo(targetPosition, toDoAfter));
        }

        public void StartMovingTo(Transform target, Action toDoAfter = null) =>
            StartMovingTo(transform.position, toDoAfter);


        private void StopMovingRoutine()
        {
            if (_moveRoutine != null)
                StopCoroutine(_moveRoutine);
        }

        private void RemoveOwner()
        {
            _previousOwner = _owner;
            _owner = null;
            transform.parent = null;
            OwnerChanged?.Invoke(_owner);
        }

        private IEnumerator MoveTo(Vector3 targetPosition, Action callback = null)
        {
            while (Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, targetPosition, _transferSpeed * Time.deltaTime);
                yield return null;
            }

            callback?.Invoke();
        }

        private IEnumerator TrackLost()
        {
            while (_owner == null)
            {
                if (_courtDimensions.CheckIfPointInsideCourt(transform.position) == false)
                    Lost?.Invoke(_previousOwner);

                yield return null;
            }
        }

        private void StartTrackFree()
        {
            StopTrackFree();
            _trackFreeRoutine = StartCoroutine(TrackFree());
        }

        private void StopTrackFree()
        {
            if (_trackFreeRoutine!=null)
                StopCoroutine(_trackFreeRoutine);
        }

        private IEnumerator TrackFree()
        {
            yield return new WaitForSeconds(_freeTimeoutSeconds);
            Free?.Invoke();
        }

        private void SetParent(Transform parent) =>
            transform.SetParent(parent, false);

        private void TurnPhysicsOn() =>
            _rigidBody.isKinematic = false;

        private void TurnPhysicsOf() =>
            _rigidBody.isKinematic = true;
    }
}