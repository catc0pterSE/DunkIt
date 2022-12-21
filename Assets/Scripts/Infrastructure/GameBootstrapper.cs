using System;
using Infrastructure.ServiceManagement;
using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private GameStateMachine _stateMachine;

        private void Awake()
        {
           _stateMachine =  new GameStateMachine(
               new SceneLoader(),
               Services.Container
           );
           _stateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
    }
}
