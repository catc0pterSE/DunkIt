using UnityEngine;

namespace Gameplay.Ball.MonoBehavior
{
    public class Ball : MonoBehaviour
    {
        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }
    }
}