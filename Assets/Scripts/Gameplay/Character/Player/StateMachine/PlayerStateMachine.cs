﻿using System;
using System.Collections.Generic;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.StateMachine.States;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine
{
    public class PlayerStateMachine : Modules.StateMachine.StateMachine
    {
        public PlayerStateMachine(PlayerFacade playerFacade)
        {
            States = new Dictionary<Type, IState>()
            {
                [typeof(ControlledAttackState)] = new ControlledAttackState(playerFacade),
                [typeof(ControlledDefenceState)] = new ControlledDefenceState(playerFacade),
                [typeof(AIControlledState)] = new AIControlledState(playerFacade), 
                [typeof(NotControlledState)] = new NotControlledState(playerFacade)
            };
        }
    }
}