using System;
using System.Collections.Generic;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.StateMachine.States;
using Infrastructure.PlayerService;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine
{
    using Ball.MonoBehavior;

    public class PlayerStateMachine : Modules.StateMachine.StateMachine
    {
        public PlayerStateMachine(PlayerFacade player, Ball ball, IPlayerService playerService)
        {
            States = new Dictionary<Type, IState>
            {
                [typeof(AttackWithBallState)] = new AttackWithBallState(player, playerService),
                [typeof(AttackWithoutBallState)] = new AttackWithoutBallState(player),
                [typeof(DefenceState)] = new DefenceState(player, playerService, ball),
                [typeof(BallChasingState)] = new BallChasingState(player, playerService, ball),
                [typeof(ThrowState)] = new ThrowState(player),
                [typeof(PassState)] = new PassState(player),
                [typeof(DunkState)] = new DunkState(player),
                [typeof(DropBallState)] = new DropBallState(player, playerService),
                [typeof(FightForBallState)] = new FightForBallState(player),
                [typeof(IdleState)] = new IdleState(player, playerService, ball),
                [typeof(NotControlledState)] = new NotControlledState(player)
            };

            Enter<BallChasingState>();
        }
    }
}