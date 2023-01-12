using System;
using Gameplay.Camera.MonoBehaviour;
using Modules.StateMachine;
using Scene;

namespace Gameplay.Camera.StateMachine.States
{
    public class DynamicCutsceneState : IParameterState<CameraRoutePoint[]>
    {
        private readonly CameraFacade _cameraFacade;

        public DynamicCutsceneState(CameraFacade cameraFacade)
        {
            _cameraFacade = cameraFacade;
        }

        public void Enter(CameraRoutePoint[] route)
        {
            _cameraFacade.DisableTargetFollowing();
            _cameraFacade.EnableFocusing();
            _cameraFacade.EnableRouteFollowing();
            _cameraFacade.RouteFollower.Run(route);
        }

        public void Exit()
        {
            _cameraFacade.DisableFocusing();
            _cameraFacade.DisableRouteFollowing();
        }
    }
}