using System;
using System.Collections;
using Modules.MonoBehaviour;
using Scene;
using UnityEngine;

namespace Gameplay.Camera.MonoBehaviour
{
    public class RouteFollower : SwitchableComponent
    {
        [SerializeField] private float _defaultMovementSpeed = 2;
        [SerializeField] private CameraFocuser _focuser;

        private Coroutine _followingRoute;

        public event Action Finished;

        public void Run(CameraRoutePoint[] route)
        {
            SetStartPosition(route[0]);

            if (_followingRoute != null)
                StopCoroutine(_followingRoute);

            _followingRoute = StartCoroutine(FollowRoute(route));
        }

        private void SetStartPosition(CameraRoutePoint startPoint)
        {
            transform.position = startPoint.transform.position;
            _focuser.SetTarget(startPoint.FocusTarget);
        }

        private IEnumerator FollowRoute(CameraRoutePoint[] route)
        {
            for (int i = 1; i < route.Length; i++)
            {
                _focuser.SetTarget(route[i].FocusTarget, true);

                while (transform.position != route[i].transform.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, route[i].transform.position,
                        _defaultMovementSpeed * Time.deltaTime);
                    yield return null;
                }
            }
            
            Finished?.Invoke();
        }
    }
}