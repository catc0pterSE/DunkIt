using System;
using System.Collections.Generic;
using UI.HUD.StateMachine.States;

namespace UI.HUD.StateMachine
{
    using Modules.StateMachine;

    public class GameplayHUDStateMachine : StateMachine
    {
        public GameplayHUDStateMachine(IGameplayHUD gameplayHUD)
        {
            States = new Dictionary<Type, IState>()
            {
                [typeof(AttackState)] = new AttackState(gameplayHUD),
                [typeof(DefenceState)] = new DefenceState(gameplayHUD)
            };
        }
    }
}