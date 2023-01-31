using System;
using System.Collections.Generic;

namespace Modules.StateMachine
{
    public abstract class StateMachine
    {
        private IState _currentState;
        protected Dictionary<Type, IState> States;
        
        public void Enter<TState>() where TState : class, IParameterlessState
        {
            if (TryChangeState(out TState state))
                state.Enter();
        }

        public void Enter<TState, TPayLoad>(TPayLoad payload) where TState : class, IParameterState<TPayLoad>
        {
            if (TryChangeState(out TState state))
                state.Enter(payload);
        }

        private TState GetState<TState>() where TState : class, IState =>
            States[typeof(TState)] as TState;

        private bool TryChangeState<TState>(out TState state) where TState : class, IState
        {
            state = GetState<TState>();

            if (state == null)
                return false;

            if (state is IParameterlessState && _currentState == state)
                return false;

            ExitCurrentState();
            SetCurrentState(state);
            return true;
        }
        
        private void ExitCurrentState() =>
            _currentState?.Exit();

        private void SetCurrentState(IState state) =>
            _currentState = state;
    }
}