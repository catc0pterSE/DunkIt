namespace Modules.StateMachine
{
    public interface IPayLoadedState<in TPayLoad> : IExitableState
    {
        public void Enter(TPayLoad payLoad);
    }
}