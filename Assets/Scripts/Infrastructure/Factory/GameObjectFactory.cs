using System;
using Gameplay.Ball.MonoBehavior;
using Gameplay.Camera.MonoBehaviour;
using Gameplay.NPC.EnemyPlayer;
using Gameplay.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.NPC.Referee.MonoBehaviour;
using Gameplay.Player.MonoBehaviour;
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

        public CameraFacade CreateCamera()
        {
            return _assetProvider.Instantiate(ResourcesPathes.CameraPath).GetComponent<CameraFacade>()
                ??  throw new NullReferenceException("No CameraFacade script on Camera prefab") ;
        }

    }
}