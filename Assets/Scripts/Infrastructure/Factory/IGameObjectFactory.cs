using Gameplay.Ball.MonoBehavior;
using Gameplay.Camera.MonoBehaviour;
using Gameplay.NPC.EnemyPlayer;
using Gameplay.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.NPC.Referee.MonoBehaviour;
using Gameplay.Player.MonoBehaviour;
using Infrastructure.ServiceManagement;
using UI;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameObjectFactory : IService
    {
        public PlayerFacade CreatePlayer();
        public EnemyFacade CreateEnemy();
        public Referee CreateReferee();
        public Ball CreateBall();
        public GameObject CreateHUD();
        public CameraFacade CreateCamera();

        public LoadingCurtain CreateLoadingCurtain();
    }
}