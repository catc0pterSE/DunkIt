using Gameplay.StateMachine.States.MinigameStates;
using Modules.StateMachine;
using UnityEngine;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    public class CelebrateCutsceneState : IParameterlessState
    {
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public CelebrateCutsceneState(GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }
        public void Exit()
        {
           
        }

        public void Enter()
        {
            _gameplayLoopStateMachine.Enter<JumpBallState>();
        }
    }
}