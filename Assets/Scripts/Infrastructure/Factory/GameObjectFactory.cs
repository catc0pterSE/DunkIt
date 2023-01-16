using System;
using Cinemachine;
using Gameplay.Ball.MonoBehavior;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Cutscene;
using Infrastructure.Provider;
using UI;
using UnityEngine;
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

        public GameObject CreateHUD()
        {
            return _assetProvider.Instantiate(ResourcesPathes.HUDPath);
        }

        public CinemachineBrain CreateCamera()
        {
            return _assetProvider.Instantiate(ResourcesPathes.CameraPath).GetComponent<CinemachineBrain>()
                ??  throw new NullReferenceException("No CameraFacade script on Camera prefab") ;
        }
    }
}