using UnityEngine;

namespace Infrastructure.Input
{
    public class SimpleInputService : IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        public Vector2 InputDirection => new Vector2
        (
            SimpleInput.GetAxis(Horizontal),
            SimpleInput.GetAxis(Vertical)
        );
    }
}