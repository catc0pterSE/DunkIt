namespace Modules.StateMachine
{
    public abstract class StateWithTransitions
    {
        protected ITransition[] Transitions;

        public virtual void Enter() =>
            EnableTransitions();

        public virtual void Exit() =>
            DisableTransitions();

        private void EnableTransitions()
        {
            if (Transitions != null)
                foreach (ITransition transition in Transitions)
                    transition.Enable();
        }

        private void DisableTransitions()
        {
            if (Transitions != null)
                foreach (ITransition transition in Transitions)
                    transition.Disable();
        }
    }
}