using System;
using System.Collections.Generic;

namespace Modules.StateMachine
{
    public abstract class StateMachine
    {
        private IState _currentState;
        protected Dictionary<Type, IState> States;

        public Type CurrentState => _currentState.GetType();
        
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
        
        public void Enter<TState, TPayLoad1, TPayLoad2>(TPayLoad1 payLoad1, TPayLoad2 payLoad2) where TState : class, IParameterState<TPayLoad1, TPayLoad2>
        {
            if (TryChangeState(out TState state))
                state.Enter(payLoad1, payLoad2);
        }

        private TState GetState<TState>() where TState : class, IState =>
            States[typeof(TState)] as TState;

        private bool TryChangeState<TState>(out TState state) where TState : class, IState
        {
            state = GetState<TState>();

            if (state == null)
                return false;

            /*if (state is IParameterlessState && _currentState == state) // TODO: do i need it?
                return false;*/

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