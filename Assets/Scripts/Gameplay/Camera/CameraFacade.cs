using Cinemachine;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Camera
{
    public class CameraFacade: SwitchableMonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private CinemachineBrain _cinemachineBrain;

        public UnityEngine.Camera Camera => _camera;

        public CinemachineBrain CinemachineBrain => _cinemachineBrain;
    }
}