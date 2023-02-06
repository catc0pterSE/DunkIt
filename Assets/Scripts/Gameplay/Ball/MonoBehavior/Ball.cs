using System;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Ball.MonoBehavior
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;

        private Character.CharacterFacade _owner;

        public Character.CharacterFacade Owner => _owner;

        public event Action<Character.CharacterFacade> OwnerChanged;

        public void SetOwner(Character.CharacterFacade owner)
        {
            TurnPhysicsOf();
            RemoveOwner();
            transform.Reset(false);
            SetParent(owner.BallPosition);
            _owner = owner;
            OwnerChanged?.Invoke(_owner);
        }

        public void Throw(Vector3 velocity)
        {
            RemoveOwner();
            TurnPhysicsOn();
            _rigidBody.AddForce(velocity, ForceMode.VelocityChange);
        }

        public void RemoveOwner()
        {
            transform.parent = null;
            _owner = null;
            OwnerChanged?.Invoke(_owner);
        }

        private void SetParent(Transform parent) =>
            transform.SetParent(parent, false);

        private void TurnPhysicsOn() =>
            _rigidBody.isKinematic = false;

        private void TurnPhysicsOf() =>
            _rigidBody.isKinematic = true;
    }
}