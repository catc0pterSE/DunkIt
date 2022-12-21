namespace Modules.StateMachine
{
    public interface IState: IExitableState
    {
        public void Enter();
    }
}