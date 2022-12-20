using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.States;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;

        private IExitableState _currentState;

        public GameStateMachine(SceneLoader sceneLoader)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            if (TryChangeState(out TState state))
                state.Enter();
        }

        public void Enter<TState, TPayLoad>(TPayLoad payload) where TState : class, IPayLoadedState<TPayLoad>
        {
            if (TryChangeState(out TState state))
                state.Enter(payload);
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;

        private bool TryChangeState<TState>(out TState state) where TState : class, IExitableState
        {
            state = GetState<TState>();

            if (_currentState == state)
                return false;

            ExitCurrentState();
            SetCurrentState(state);
            return true;
        }

        private void ExitCurrentState() =>
            _currentState?.Exit();

        private void SetCurrentState(IExitableState state) =>
            _currentState = state;
    }
}