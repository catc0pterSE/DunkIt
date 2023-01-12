using System;
using System.Collections.Generic;
using Gameplay.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.NPC.EnemyPlayer.StateMachine.States;
using Modules.StateMachine;

namespace Gameplay.NPC.EnemyPlayer.StateMachine
{
    public class EnemyStateMachine : Modules.StateMachine.StateMachine
    {
        public EnemyStateMachine(EnemyFacade enemyFacade)
        {
            States = new Dictionary<Type, IState>()
            {
                [typeof(CutsceneState)] = new CutsceneState(enemyFacade),
            };
        }
    }
}