using Cinemachine;
using UnityEngine;
using Utility.Constants;

namespace Utility.Extensions
{
    public static class CinemachineVirtualCameraExtensions
    {
        public const int CinemachineActualCameraOrder = 15;
        public const int CinemachineDefaultCameraOrder = 0;
        
        public static void Prioritize(this CinemachineVirtualCamera virtualCamera)
        {
            CinemachineVirtualCamera[] virtualCameras = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
            
            foreach (CinemachineVirtualCamera camera in virtualCameras)
            {
                camera.Deprioritize();
            }
            
            virtualCamera.Priority = CinemachineActualCameraOrder;
        }
        
        private static void Deprioritize(this CinemachineVirtualCamera virtualCamera)
        {
            virtualCamera.Priority = CinemachineDefaultCameraOrder;
        }
    }
}