using Modules.MonoBehaviour;

namespace Gameplay.Character
{
    using Ball.MonoBehavior;
    public abstract class Character: SwitchableMonoBehaviour
    {
        private Ball _ball;
        public void TakeBall() =>
            _ball.SetParent(transform);

        protected void SetBall(Ball ball) =>
            _ball = ball;
    }
}