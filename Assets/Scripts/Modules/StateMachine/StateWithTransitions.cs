namespace Modules.StateMachine
{
    public abstract class StateWithTransitions : IParameterlessState
    {
        private readonly ITransition[] _transitions;

        protected StateWithTransitions(ITransition[] transitions)
        {
            _transitions = transitions;
        }

        public virtual void Enter()
        {
            EnableTransitions();
        }
        
        public virtual void Exit()
        {
            DisableTransitions();
        }

        private void EnableTransitions()
        {
            foreach (ITransition transition in _transitions)
                transition.Enable();
        }

        private void DisableTransitions()
        {
            foreach (ITransition transition in _transitions)
                transition.Disable();
        }
    }
}