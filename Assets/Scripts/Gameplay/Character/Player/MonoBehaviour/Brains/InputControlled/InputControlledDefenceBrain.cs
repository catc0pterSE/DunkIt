using System;

namespace Gameplay.Character.Player.MonoBehaviour.Brains
{
    public class InputControlledDefenceBrain : InputControlledBrain
    {
        public event Action ChangePlayerInitiated;

        private void OnEnable() =>
            InputService.ChangePlayerButtonDown += InitiateChangePlayer;

        private void OnDisable() =>
            InputService.ChangePlayerButtonDown -= InitiateChangePlayer;

        private void InitiateChangePlayer() =>
            ChangePlayerInitiated?.Invoke();
    }
}