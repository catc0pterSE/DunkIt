namespace Infrastructure.StateMachine.States
{
    public interface IPayLoadedState<in TPayLoad> : IExitableState
    {
        public void Enter(TPayLoad payLoad);
    }
}