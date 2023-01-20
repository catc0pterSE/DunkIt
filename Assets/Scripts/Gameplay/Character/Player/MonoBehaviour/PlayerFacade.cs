using Cinemachine;
using Gameplay.Character.Player.MonoBehaviour.Brains;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using Gameplay.Character.Player.StateMachine;
using UnityEngine;
using Utility.Constants;

namespace Gameplay.Character.Player.MonoBehaviour
{
    using Ball.MonoBehavior;

    public class PlayerFacade : Character
    {
        [SerializeField] private InputControlledBrain _inputControlledBrain;
        [SerializeField] private AIControlledBrain _aiControlledBrain;
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private Animator _animator;

        private CinemachineVirtualCamera _virtualCamera;
        private PlayerStateMachine _stateMachine;
        public PlayerStateMachine StateMachine => _stateMachine ??= new PlayerStateMachine(this);
        public Animator Animator => _animator;

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

        public void Initialize(Ball ball, Transform gameplayCamera, CinemachineVirtualCamera virtualCamera)
        {
            SetBall(ball);
            _inputControlledBrain.SetCamera(gameplayCamera.transform);
            _virtualCamera = virtualCamera;
            _virtualCamera.Follow = transform;
        }

        public void PrioritizeCamera(Transform lookTarget)
        {
            _virtualCamera.Priority = NumericConstants.CinemachineActualCameraOrder;
            _virtualCamera.LookAt = lookTarget;
        }

        public void DeprioritizeCamera() =>
            _virtualCamera.Priority = NumericConstants.CinemachineDefaultCameraOrder;
    }
}