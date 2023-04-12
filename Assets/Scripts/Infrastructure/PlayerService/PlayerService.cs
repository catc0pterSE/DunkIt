using System;
using System.Collections.Generic;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.StateMachine.States;
using Modules.LiveData;
using UI.HUD.StateMachine;

namespace Infrastructure.PlayerService
{
    public class PlayerService : IPlayerService, IHUDStateController
    {
        private MutableLiveData<Action<GameplayHUDStateMachine>> _hudStateSelection = new MutableLiveData<Action<GameplayHUDStateMachine>>();

        private Dictionary<Type, Action<GameplayHUDStateMachine>> _typedHUDStateSelections;
        
        public PlayerService()
        {
            _typedHUDStateSelections = new Dictionary<Type, Action<GameplayHUDStateMachine>>
            {
                [typeof(AttackWithBallState)] = (stateMachine) => stateMachine.Enter<UI.HUD.StateMachine.States.AttackWithBallState, PlayerFacade>(CurrentControlled),
                [typeof(DefenceState)] = (stateMachine) => stateMachine.Enter<UI.HUD.StateMachine.States.DefenceState, PlayerFacade>(CurrentControlled),
                [typeof(BallChasingState)] = (stateMachine) => stateMachine.Enter<UI.HUD.StateMachine.States.BallChasingState, PlayerFacade>(CurrentControlled),
                [typeof(ThrowState)] = (stateMachine) => stateMachine.Enter<UI.HUD.StateMachine.States.ThrowState, PlayerFacade>(CurrentControlled),
                [typeof(DropBallState)] = (stateMachine) => stateMachine.Enter<UI.HUD.StateMachine.States.DropBallState, PlayerFacade>(CurrentControlled),
                [typeof(IdleState)] = (stateMachine) => stateMachine.Enter<UI.HUD.StateMachine.States.IdleState, PlayerFacade>(CurrentControlled)
            };
        }

        public LiveData<Action<GameplayHUDStateMachine>> HudStateSelection => _hudStateSelection;

        public PlayerFacade CurrentControlled { get; private set; }

        public void Set(PlayerFacade player)
        {
            CurrentControlled = player;
            _hudStateSelection.Value = _typedHUDStateSelections[player.CurrentStateType];
        }
    }
}