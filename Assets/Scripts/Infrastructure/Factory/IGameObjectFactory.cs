using Cinemachine;
using Gameplay.Ball.MonoBehavior;
using Gameplay.Camera;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Cutscene;
using Gameplay.Minigame.FightForBall;
using Gameplay.Minigame.JumpBall;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using UI;
using UI.HUD.Controls.Mobile;

namespace Infrastructure.Factory
{
    public interface IGameObjectFactory : IService
    {
        public PlayerFacade CreatePlayer();
        public Referee CreateReferee();
        public Ball CreateBall();
        public MobileControlsHUDView CreateMobileControlsHUD();
        public CameraFacade CreateCamera();
        public LoadingCurtain CreateLoadingCurtain();
        public StartCutscene CreateStartCutscene();
        public CinemachineVirtualCamera CreateCinemachineVirtualCamera();
        public JumpBallMinigame CreateJumpBallMinigame();
        public FightForBallMinigame CreateFightForBallMinigame();
        public MobileInputService CreateMobileInputService();
    }
}