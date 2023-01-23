using Modules.LiveData;
using UnityEngine;

namespace Gameplay.Ball.MonoBehavior
{
    using Character;

    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private MeshRenderer[] _renderers;

        private readonly MutableLiveData<Character> _ownerData = new MutableLiveData<Character>();
        private bool _isGhost;
        public LiveData<Character> OwnerData => _ownerData;

        public void SetOwner(Character owner)
        {
            TurnPhysicsOf();

            if (_ownerData.Value != null)
                _ownerData.Value.LoseBall();
            
            transform.SetParent(owner.BallPosition.transform, false);
            owner.TakeBall();
            _ownerData.Value = owner;
        }

        public void Throw(Vector3 velocity)
        {
            RemoveParent();
            TurnPhysicsOn();
            _rigidBody.AddForce(velocity, ForceMode.Impulse);
        }

        public void StopRendering()
        {
            foreach (MeshRenderer meshRenderer in _renderers)
                meshRenderer.enabled = false;
        }

        public void ZeroVelocity() =>
            _rigidBody.velocity = Vector3.zero;

        private void RemoveParent()
        {
            transform.parent = null;
            _ownerData.Value = null;
        }

        private void TurnPhysicsOn() =>
            _rigidBody.isKinematic = false;

        private void TurnPhysicsOf() =>
            _rigidBody.isKinematic = true;
    }
}