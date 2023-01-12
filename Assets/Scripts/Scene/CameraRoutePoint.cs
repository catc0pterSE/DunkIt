using System;
using UnityEngine;

namespace Scene
{
    public class CameraRoutePoint : MonoBehaviour
    {
        [SerializeField] private Transform _focusTarget;

        public Transform FocusTarget => _focusTarget;
    }
}