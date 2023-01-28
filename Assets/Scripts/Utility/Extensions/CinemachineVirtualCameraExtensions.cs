using Cinemachine;
using Utility.Constants;

namespace Utility.Extensions
{
    public static class CinemachineVirtualCameraExtensions
    {
        public static void Prioritize(this CinemachineVirtualCamera virtualCamera)
        {
            virtualCamera.Priority = NumericConstants.CinemachineActualCameraOrder;
        }
        
        public static void Deprioritize(this CinemachineVirtualCamera virtualCamera)
        {
            virtualCamera.Priority = NumericConstants.CinemachineDefaultCameraOrder;
        }
    }
}