using System;
using System.Collections.Generic;

namespace Modules.StateMachine
{
    public abstract class StateMachine
    {
        private IState _currentState;
        protected Dictionary<Type, IState> States;

        public Type CurrentState => _currentState.GetType();

        public event Action StateChanged;

        public void Enter<TState>() where TState : class, IParameterlessState
        {
            if (TryChangeState(out TState state) == false)
                return;
            
            state.Enter();
            StateChanged?.Invoke();
        }

        public void Enter<TState, TPayLoad>(TPayLoad payload) where TState : class, IParameterState<TPayLoad>
        {
            if (TryChangeState(out TState state) == false)
                return;
            
            state.Enter(payload);
            StateChanged?.Invoke();
        }
        
        public void Enter<TState, TPayLoad1, TPayLoad2>(TPayLoad1 payLoad1, TPayLoad2 payLoad2) where TState : class, IParameterState<TPayLoad1, TPayLoad2>
        {
            if (TryChangeState(out TState state) == false)
                return;
            
            state.Enter(payLoad1, payLoad2);
            StateChanged?.Invoke();
        }

        private TState GetState<TState>() where TState : class, IState =>
            States[typeof(TState)] as TState;

        private bool TryChangeState<TState>(out TState state) where TState : class, IState
        {
            state = GetState<TState>();

            if (state == null)
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