using System;
using System.Collections.Generic;
using Infrastructure.Factory;
using Infrastructure.ServiceManagement;
using Infrastructure.StateMachine.States;
using Modules.StateMachine;

namespace Infrastructure.StateMachine
{
    using StateMachine = Modules.StateMachine.StateMachine;

    public class GameStateMachine : StateMachine
    {
        public GameStateMachine(SceneLoader sceneLoader, Services services
        )
        {
            States = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, services.Single<IGameObjectFactory>()),
                [typeof(GamePlayState)] = new GamePlayState(this)
            };
        }
    }
}