using System;
using System.Collections.Generic;
using Gameplay.Camera.MonoBehaviour;
using Gameplay.Camera.StateMachine.States;
using Modules.StateMachine;

namespace Gameplay.Camera.StateMachine
{
    public class CameraStateMachine : Modules.StateMachine.StateMachine
    {
        public CameraStateMachine(CameraFacade cameraFacade)
        {
            States = new Dictionary<Type, IState>()
            {
                [typeof(StaticCutSceneState)] = new DynamicCutsceneState(cameraFacade),
            };
        }
    }
}