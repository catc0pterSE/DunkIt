namespace Modules.StateMachine
{
    public abstract class Transition<TState> : ITransition where TState: class, IParameterlessState 
    {
        private readonly StateMachine _stateMachine;

        protected Transition(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        protected void MoveToNextState()
        {
            _stateMachine.Enter<TState>();
        }

        public abstract void Enable();

        public abstract void Disable();
    }
}