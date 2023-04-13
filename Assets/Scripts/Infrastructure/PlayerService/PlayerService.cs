using System;
using System.Collections.Generic;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.StateMachine.States;
using Modules.LiveData;
using UI.HUD.Controls.StateMachine;

namespace Infrastructure.PlayerService
{
    public class PlayerService : IPlayerService, IControlsHUDStateController
    {
        private MutableLiveData<Action<GameplayHUDStateMachine>> _hudStateSelection =
            new MutableLiveData<Action<GameplayHUDStateMachine>>();

        private Dictionary<Type, Action<GameplayHUDStateMachine>> _typedHUDStateSelections;

        public PlayerService()
        {
            _typedHUDStateSelections = new Dictionary<Type, Action<GameplayHUDStateMachine>>
            {
                [typeof(AttackWithBallState)] = (stateMachine) => stateMachine.Enter<UI.HUD.Controls.StateMachine.States.AttackWithBallState, PlayerFacade>(CurrentControlled),
                [typeof(DefenceState)] = (stateMachine) => stateMachine.Enter<UI.HUD.Controls.StateMachine.States.DefenceState>(),
                [typeof(BallChasingState)] = (stateMachine) => stateMachine.Enter<UI.HUD.Controls.StateMachine.States.BallChasingState>(),
                [typeof(DropBallState)] = (stateMachine) => stateMachine.Enter<UI.HUD.Controls.StateMachine.States.DropBallState, PlayerFacade>(CurrentControlled),
                [typeof(ThrowState)] = (stateMachine) => stateMachine.Enter<UI.HUD.Controls.StateMachine.States.OffState>(),
                [typeof(IdleState)] = (stateMachine) => stateMachine.Enter<UI.HUD.Controls.StateMachine.States.OffState>(),
                [typeof(NotControlledState)] = (stateMachine) => stateMachine.Enter<UI.HUD.Controls.StateMachine.States.OffState>(),
                [typeof(DunkState)] = (stateMachine) => stateMachine.Enter<UI.HUD.Controls.StateMachine.States.OffState>(),
                [typeof(AttackWithoutBallState)] = (stateMachine) => stateMachine.Enter<UI.HUD.Controls.StateMachine.States.OffState>(),
                [typeof(FightForBallState)] = (stateMachine) => stateMachine.Enter<UI.HUD.Controls.StateMachine.States.OffState>(),
                [typeof(PassState)] = (stateMachine) => stateMachine.Enter<UI.HUD.Controls.StateMachine.States.OffState>(),
            };
        }

        public LiveData<Action<GameplayHUDStateMachine>> HudStateSelection => _hudStateSelection;

        public PlayerFacade CurrentControlled { get; private set; }

        public void Set(PlayerFacade player)
        {
            if (CurrentControlled != null)
                UnsubscribeFromCurrentPlayer();

            CurrentControlled = player;
            UpdateControlsHUDState();

            SubscribeOnCurrentPlayer();
        }

        private void UpdateControlsHUDState() =>
            _hudStateSelection.Value = _typedHUDStateSelections[CurrentControlled.CurrentStateType];

        private void SubscribeOnCurrentPlayer() =>
            CurrentControlled.StateChanged += UpdateControlsHUDState;

        private void UnsubscribeFromCurrentPlayer() =>
            CurrentControlled.StateChanged -= UpdateControlsHUDState;
    }
}