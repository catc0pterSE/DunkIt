using Gameplay.Player.MonoBehaviour.Brain;
using Gameplay.Player.MonoBehaviour.Movement;
using UnityEngine;

namespace Gameplay.Player.MonoBehaviour
{
    using MonoBehaviour = UnityEngine.MonoBehaviour;

    public class PlayerFacade : MonoBehaviour
    {
        [SerializeField] private InputControlledBrain _inputControlledBrain;
        [SerializeField] private AIControlledBrain _aiControlledBrain;
        [SerializeField] private PlayerMover _playerMover;

        public void EnableInputControlledBrain() =>
            _inputControlledBrain.Enable();

        public void DisableInputControlledBrain() =>
            _inputControlledBrain.Disable();

        public void EnableAIControlledBrain() =>
            _aiControlledBrain.Enable();

        public void DisableAIControlledBrain() =>
            _aiControlledBrain.Disable();

        public void EnablePlayerMover() =>
            _playerMover.Enable();

        public void DisablePlayerMover() =>
            _playerMover.Disable();
    }
}