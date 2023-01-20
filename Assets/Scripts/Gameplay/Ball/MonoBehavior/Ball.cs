using Modules.LiveData;
using UnityEngine;

namespace Gameplay.Ball.MonoBehavior
{
    using Character;

    public class Ball : MonoBehaviour
    {
        private readonly MutableLiveData<Character> _ownerData = new MutableLiveData<Character>();

        public LiveData<Character> OwnerData => _ownerData;

        public void SetOwner(Character parent)
        {
            transform.SetParent(parent.BallPosition.transform, false);
            _ownerData.Value = parent;
        }

        public void RemoveParent()
        {
            transform.parent = null;
            _ownerData.Value = null;
        }
    }
}