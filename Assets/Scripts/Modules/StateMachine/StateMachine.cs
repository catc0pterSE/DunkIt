using System;
using System.Collections.Generic;

namespace Modules.StateMachine
{
    public abstract class StateMachine
    {
        private IExitableState _currentState;

        protected Dictionary<Type, IExitableState> States; 
     
        
        
        
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
            States[typeof(TState)] as TState;

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