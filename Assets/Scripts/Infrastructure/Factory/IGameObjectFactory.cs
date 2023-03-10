using Cinemachine;
using Gameplay.Ball.MonoBehavior;
using Gameplay.Camera;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Cutscene;
using Gameplay.Minigame.JumpBall;
using Infrastructure.ServiceManagement;
using UI;
using UI.HUD.Mobile;

namespace Infrastructure.Factory
{
    public interface IGameObjectFactory : IService
    {
        public PlayerFacade CreatePlayer();
        public EnemyFacade CreateEnemy();
        public Referee CreateReferee();
        public Ball CreateBall();
        public MobileGameplayHUD CreateMobileHUD();
        public CameraFacade CreateCamera();
        public LoadingCurtain CreateLoadingCurtain();
        public StartCutscene CreateStartCutscene();
        public CinemachineVirtualCamera CreateCinemachineVirtualCamera();
        public JumpBallMinigame CreateJumpBallMinigame();
    }
}