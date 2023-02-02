using System;
using Cinemachine;
using Gameplay.Ball.MonoBehavior;
using Gameplay.Camera;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Cutscene;
using Gameplay.Minigame;
using Gameplay.Minigame.JumpBall;
using Gameplay.Minigame.Throw;
using Infrastructure.Input.InputService;
using Infrastructure.Provider;
using UI;
using UI.HUD;
using UI.HUD.Mobile;
using Utility.Constants;

namespace Infrastructure.Factory
{
    public class GameObjectFactory : IGameObjectFactory
    {
        private readonly IAssetProvider _assetProvider;

        public GameObjectFactory(IAssetProvider assetProvider)
        {
           _assetProvider = assetProvider;
        }
        
        public LoadingCurtain CreateLoadingCurtain()
        {
            return _assetProvider.Instantiate(ResourcesPathes.LoadingCurtainPath).GetComponent<LoadingCurtain>()
                ?? throw new NullReferenceException("No LoadingCurtain script on LoadingCurtain prefab");
        }

        public StartCutscene CreateStartCutscene()
        {
            return _assetProvider.Instantiate(ResourcesPathes.StartCutscenePath).GetComponent<StartCutscene>()
                   ?? throw new NullReferenceException("No LoadingCurtain script on LoadingCurtain prefab");
        }

        public PlayerFacade CreatePlayer()
        {
            return _assetProvider.Instantiate(ResourcesPathes.PlayerPath).GetComponent<PlayerFacade>()
                ?? throw new NullReferenceException("No PlayerFacade script on Player prefab");
        }

        public EnemyFacade CreateEnemy()
        {
            return _assetProvider.Instantiate(ResourcesPathes.EnemyPath).GetComponent<EnemyFacade>()
                   ?? throw new NullReferenceException("No EnemyFacade script on Enemy prefab");
        }

        public Referee CreateReferee()
        {
            return _assetProvider.Instantiate(ResourcesPathes.RefereePath).GetComponent<Referee>()
                   ?? throw new NullReferenceException("No Referee script on Referee prefab");
        }

        public Ball CreateBall()
        {
            return _assetProvider.Instantiate(ResourcesPathes.BallPath).GetComponent<Ball>()
                   ?? throw new NullReferenceException("No Ball script on Ball prefab");
        }

        public MobileGameplayHUD CreateMobileHUD()
        {
            return _assetProvider.Instantiate(ResourcesPathes.MobileHUDPath).GetComponent<MobileGameplayHUD>()
                   ?? throw new NullReferenceException("No GameplayHUD script on GameplayHUD prefab");
        }

        public CameraFacade CreateCamera()
        {
            return _assetProvider.Instantiate(ResourcesPathes.CameraPath).GetComponent<CameraFacade>()
                ??  throw new NullReferenceException("No CameraFacade script on Camera prefab");
        }

        public CinemachineVirtualCamera CreateCinemachineVirtualCamera()
        {
            return _assetProvider.Instantiate(ResourcesPathes.CinemachineVirtualCamera).GetComponent<CinemachineVirtualCamera>()
                   ??  throw new NullReferenceException("No CinemachineVirtualCamera component on CinemachineVirtualCamera prefab");
        }

        public JumpBallMinigame CreateJumpBallMinigame()
        {
            return _assetProvider.Instantiate(ResourcesPathes.JumpBallMinigamePath).GetComponent<JumpBallMinigame>()
                   ??  throw new NullReferenceException("No JumpBallMinigame component on JumpBallMinigame prefab");
        }

        public ThrowMinigame CreateThrowMinigame()
        {
            return _assetProvider.Instantiate(ResourcesPathes.ThrowMinigamePath).GetComponent<ThrowMinigame>()
                   ??  throw new NullReferenceException("No ThrowMinigame component on ThrowMinigame prefab");
        }

        public MobileInputService CreateMobileInputService()
        {
            return _assetProvider.Instantiate(ResourcesPathes.MobileInputService).GetComponent<MobileInputService>()
                   ??  throw new NullReferenceException("No MobileInputService component on MobileInputService prefab");
        }
    }
}