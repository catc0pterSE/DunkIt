using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character
{
    using Ball.MonoBehavior;
    public abstract class Character: SwitchableMonoBehaviour
    {
        [SerializeField] private Transform _ballPosition;

        private Ball _ball;
        
        public Transform BallPosition => _ballPosition;
       
        public void TakeBall() =>
            _ball.SetOwner(this);

        protected void SetBall(Ball ball) =>
            _ball = ball;
    }
}