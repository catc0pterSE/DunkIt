using Cinemachine;
using UnityEngine;
using Utility.Constants;

namespace Utility.Extensions
{
    public static class CinemachineVirtualCameraExtensions
    {
        public static void Prioritize(this CinemachineVirtualCamera virtualCamera)
        {
            CinemachineVirtualCamera[] virtualCameras = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
            
            foreach (CinemachineVirtualCamera camera in virtualCameras)
            {
                camera.Deprioritize();
            }
            
            virtualCamera.Priority = NumericConstants.CinemachineActualCameraOrder;
        }
        
        private static void Deprioritize(this CinemachineVirtualCamera virtualCamera)
        {
            virtualCamera.Priority = NumericConstants.CinemachineDefaultCameraOrder;
        }
    }
}