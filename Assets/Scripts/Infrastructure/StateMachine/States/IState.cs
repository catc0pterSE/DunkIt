namespace Infrastructure.StateMachine
{
    public interface IState: IExitableState
    {
        public void Enter();
    }
}