using Gameplay.Camera.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Camera.StateMachine.States
{
    public class DynamicCutsceneState : IParameterlessState
    {
        private readonly CameraFacade _cameraFacade;

        public DynamicCutsceneState(CameraFacade cameraFacade)
        {
            _cameraFacade = cameraFacade;
        }

        public void Enter()
        {
            _cameraFacade.DisableTargetFollowing();
            _cameraFacade.EnableFocusing();
        }
        
        public void Exit()
        {
            _cameraFacade.DisableFocusing();
        }
    }
}