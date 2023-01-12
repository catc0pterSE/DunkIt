using System;
using System.Collections;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Camera.MonoBehaviour
{
    public class RouteFollower: SwitchableComponent
    {
        [SerializeField] private float _defaultMovementSpeed = 2;

        private Coroutine _followingRoute;
            
        public event Action<int> Reached;
        public event Action Finished;

        public void Run(Vector3[] route)
        {
            SetStartPosition(route[0]);

            if (_followingRoute !=null)
                StopCoroutine(_followingRoute);
            
            _followingRoute = StartCoroutine(FollowRoute(route));
        }

        private void SetStartPosition(Vector3 position)
        {
            transform.position = position;
        }

        private IEnumerator FollowRoute(Vector3[] route)
        {
            for (int i = 1; i < route.Length; i++)
            {
                while (transform.position != route[i])
                {
                    transform.position = Vector3.MoveTowards(transform.position, route[i],
                        _defaultMovementSpeed * Time.deltaTime);
                    yield return null;
                }
                
                Reached?.Invoke(i);
            }

            Finished?.Invoke();
            gameObject.SetActive(false);
        }
    }
}