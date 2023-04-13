using Gameplay.StateMachine.States.Gameplay;
using Modules.StateMachine;
using Scene;
using Scene.Ring;
using Utility.Extensions;

namespace Gameplay.StateMachine.Transitions
{
    public class AnyToGoalState: ITransition
    {
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;
        private readonly Ring[] _rings;

        public AnyToGoalState(SceneInitials sceneInitials, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
            _rings = new[] { sceneInitials.LeftRing, sceneInitials.RightRing };
        }


        public void Enable()
        {
            SubscribeOnRings();
        }

        public void Disable()
        {
            UnsubscribeFromRings();
        }

        private void SubscribeOnRings() =>
            _rings.Map(ring => ring.Goal += OnGoalScored);
        
        private void UnsubscribeFromRings() =>
            _rings.Map(ring => ring.Goal += OnGoalScored);

        private void OnGoalScored(Ring ring)
        {
            _gameplayLoopStateMachine.Enter<GoalState, Ring>(ring);
        }
    }
}