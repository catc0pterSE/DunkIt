using System;
using System.Collections.Generic;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.EnemyPlayer.StateMachine.States;
using Modules.StateMachine;

namespace Gameplay.Character.NPC.EnemyPlayer.StateMachine
{
    public class EnemyStateMachine : Modules.StateMachine.StateMachine
    {
        public EnemyStateMachine(EnemyFacade enemy)
        {
            States = new Dictionary<Type, IState>()
            {
                [typeof(NotControlledState)] = new NotControlledState(enemy),
                [typeof(AIControlledState)] = new AIControlledState(),
                [typeof(IdleState)] = new IdleState(),
                [typeof(ContestingBallState)] = new ContestingBallState()
            };
        }
    }
}