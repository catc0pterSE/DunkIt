using System;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Ball.MonoBehavior
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;

        private Character.Character _owner;

        public Character.Character Owner => _owner;

        public event Action<Character.Character> OwnerChanged;
     
        public void SetOwner(Character.Character owner)
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

        private void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        private void RemoveOwner()
        {
            _owner = null;
            
            OwnerChanged?.Invoke(_owner);
        }

        private void TurnPhysicsOn() =>
            _rigidBody.isKinematic = false;

        private void TurnPhysicsOf() =>
            _rigidBody.isKinematic = true;
    }
}