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
                [typeof(AttackWithBallState)] = new AttackWithBallState(gameplayHUD),
                [typeof(DefenceState)] = new DefenceState(gameplayHUD),
                [typeof(BallChasingState)] = new BallChasingState(gameplayHUD),
                [typeof(DropBallState)] = new DropBallState(gameplayHUD),
                [typeof(ThrowState)] = new ThrowState(gameplayHUD),
                [typeof(IdleState)] = new IdleState(gameplayHUD)
            };
        }
    }
}