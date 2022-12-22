using System;
using System.Collections.Generic;
using Gameplay.Player.MonoBehaviour;
using Gameplay.Player.StateMachine.States;
using Modules.StateMachine;

namespace Gameplay.Player.StateMachine
{
    public class PlayerStateMachine : Modules.StateMachine.StateMachine
    {
        public PlayerStateMachine(PlayerFacade playerFacade)
        {
            States = new Dictionary<Type, IState>()
            {
                [typeof(ControlledState)] = new ControlledState(playerFacade),
                [typeof(AIState)] = new AIState(playerFacade)
            };
        }
    }
}