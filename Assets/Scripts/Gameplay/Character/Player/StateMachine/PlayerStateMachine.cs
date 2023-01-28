using System;
using System.Collections.Generic;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.StateMachine.States;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine
{
    public class PlayerStateMachine : Modules.StateMachine.StateMachine
    {
        public PlayerStateMachine(PlayerFacade player)
        {
            States = new Dictionary<Type, IState>()
            {
                [typeof(ControlledAttackState)] = new ControlledAttackState(player),
                [typeof(ControlledDefenceState)] = new ControlledDefenceState(player),
                [typeof(AIControlledState)] = new AIControlledState(player), 
                [typeof(NotControlledState)] = new NotControlledState(player)
            };
        }
    }
}