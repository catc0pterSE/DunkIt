using System;
using System.Collections.Generic;
using Gameplay.Character.Player.StateMachine.States;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine
{
    public class PlayerStateMachine : Modules.StateMachine.StateMachine
    {
        public PlayerStateMachine(MonoBehaviour.PlayerFacade player)
        {
            States = new Dictionary<Type, IState>()
            {
                [typeof(InputControlledAttackState)] = new InputControlledAttackState(player),
                [typeof(InputControlledDefenceState)] = new InputControlledDefenceState(player),
                [typeof(AIControlledAttackWithBallState)] = new AIControlledAttackWithBallState(),
                [typeof(AIControlledAttackWithoutBallState)] = new AIControlledAttackWithoutBallState(player),
                [typeof(AIControlledDefenceState)] = new AIControlledDefenceState(),
                [typeof(ThrowState)] = new ThrowState(player),
                [typeof(IdleState)] = new IdleState(player),
                [typeof(PassState)] = new PassState(player),
                [typeof(CatchState)] = new CatchState(player),
                [typeof(DunkState)] = new DunkState(player),
                [typeof(FightForBallState)] = new FightForBallState(player),
                [typeof(NotControlledState)] = new NotControlledState(player)
            };
        }
    }
}