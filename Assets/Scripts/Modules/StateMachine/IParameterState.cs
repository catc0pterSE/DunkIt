namespace Modules.StateMachine
{
    public interface IParameterState<TPayLoad> : IState
    {
        public void Enter(TPayLoad payLoad);
    }
}