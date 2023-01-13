using System;
using Gameplay.Camera.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Camera.StateMachine.States
{
    public class CutsceneState : IParameterlessState
    {
        private readonly CameraFacade _cameraFacade;

        public CutsceneState(CameraFacade cameraFacade)
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