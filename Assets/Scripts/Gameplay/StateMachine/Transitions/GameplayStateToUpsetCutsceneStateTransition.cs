using Gameplay.StateMachine.States.Gameplay;
using Modules.StateMachine;
using Scene.Ring;

namespace Gameplay.StateMachine.Transitions
{
    public class GameplayStateToUpsetCutsceneStateTransition : ITransition
    {
        private readonly Ring _playerRing;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        public GameplayStateToUpsetCutsceneStateTransition
        (
            Ring playerRing,
            GameplayLoopStateMachine gameplayLoopStateMachine
        )
        {
            _playerRing = playerRing;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
        }

        public void Enable()
        {
            SubscribeOnEnemyRing();
        }

        public void Disable()
        {
            UnsubscribeFromEnemyRing();
        }

        private void SubscribeOnEnemyRing() =>
            _playerRing.Goal += OnGoalScored;

        private void UnsubscribeFromEnemyRing() =>
            _playerRing.Goal -= OnGoalScored;

        private void OnGoalScored() => MoveToUpsetCutscene();

        private void MoveToUpsetCutscene() => 
        _gameplayLoopStateMachine.Enter<GameplayState>();
        
    }
}