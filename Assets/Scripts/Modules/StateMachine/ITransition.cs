namespace Modules.StateMachine
{
    public interface ITransition
    {
        public void Enable();
        public void Disable();
    }
}