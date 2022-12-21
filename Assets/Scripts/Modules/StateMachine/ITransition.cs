using System;

namespace Modules.StateMachine
{
    public interface ITransition
    {
        public event Action MoveToNextState;
    }
}