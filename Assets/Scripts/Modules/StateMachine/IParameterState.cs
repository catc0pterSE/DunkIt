namespace Modules.StateMachine
{
    public interface IParameterState<TPayLoad> : IState
    {
        public void Enter(TPayLoad payload);
    }

    public interface IParameterState<TPayLoad1, TPayload2> : IState
    {
        public void Enter(TPayLoad1 payload1, TPayload2 payload2);
    }
}