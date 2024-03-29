using Agava.YandexGames;
using Infrastructure.CoroutineRunner;
using Infrastructure.ServiceManagement;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private GameStateMachine _stateMachine;

        private void Awake()
        {
            YandexGamesSdk.Initialize();
            
            _stateMachine = new GameStateMachine(
                new SceneLoader(),
                Services.Container,
                this
            );
            _stateMachine.Enter<BootstrapState>();
            

            DontDestroyOnLoad(this);
        }
    }
}