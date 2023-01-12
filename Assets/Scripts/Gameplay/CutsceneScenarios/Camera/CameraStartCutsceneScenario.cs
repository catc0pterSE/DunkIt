using Gameplay.Camera.MonoBehaviour;
using Scene;

namespace Gameplay.CutsceneScenarios.Camera
{
    public class CameraStartCutsceneScenario
    {
        private readonly SceneConfig _sceneConfig;
        private readonly CameraFacade _cameraFacade;

        public CameraStartCutsceneScenario(SceneConfig sceneConfig, CameraFacade cameraFacade)
        {
            _sceneConfig = sceneConfig;
            _cameraFacade = cameraFacade;
        }
        
        
    }
}