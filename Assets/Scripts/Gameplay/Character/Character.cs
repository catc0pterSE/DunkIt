using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character
{
    using Ball.MonoBehavior;
    public abstract class Character: SwitchableMonoBehaviour
    {
        [SerializeField] private Transform _ballPosition;
        
        private Ball _ball;
        public void TakeBall() =>
            _ball.SetParent(_ballPosition);

        protected void SetBall(Ball ball) =>
            _ball = ball;
    }
}