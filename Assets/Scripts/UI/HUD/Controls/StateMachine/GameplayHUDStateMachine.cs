using System;
using System.Collections.Generic;
using Modules.StateMachine;
using UI.HUD.Controls.StateMachine.States;

namespace UI.HUD.Controls.StateMachine
{
    public class GameplayHUDStateMachine : Modules.StateMachine.StateMachine
    {
        public GameplayHUDStateMachine(IControlsHUDView controlsHUDView)
        {
            States = new Dictionary<Type, IState>()
            {
                [typeof(AttackWithBallState)] = new AttackWithBallState(controlsHUDView),
                [typeof(DefenceState)] = new DefenceState(controlsHUDView),
                [typeof(BallChasingState)] = new BallChasingState(controlsHUDView),
                [typeof(DropBallState)] = new DropBallState(controlsHUDView),
                [typeof(OffState)] = new OffState(controlsHUDView)
            };
        }
    }
}