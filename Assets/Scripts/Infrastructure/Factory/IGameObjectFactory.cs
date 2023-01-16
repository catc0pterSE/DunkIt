using Cinemachine;
using Gameplay.Ball.MonoBehavior;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Cutscene;
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
        public CinemachineBrain CreateCamera();
        public LoadingCurtain CreateLoadingCurtain();
        public StartCutscene CreateStartCutscene();
    }
}