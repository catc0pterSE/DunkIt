namespace Infrastructure.StateMachine.States
{
    public interface IState: IExitableState
    {
        public void Enter();
    }
}