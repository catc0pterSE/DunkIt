namespace Modules.StateMachine
{
    public abstract class Transition<TState> where TState: class, IParameterlessState
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
    }
}