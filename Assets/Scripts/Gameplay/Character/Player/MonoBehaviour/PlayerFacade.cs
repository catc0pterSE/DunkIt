using System;
using Cinemachine;
using Gameplay.Character.Player.BallHandle.Throw;
using Gameplay.Character.Player.MonoBehaviour.Brains;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using Gameplay.Character.Player.StateMachine;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;
using z_Test;
using SceneConfig = Scene.SceneConfig;

namespace Gameplay.Character.Player.MonoBehaviour
{
    using Ball.MonoBehavior;
    using Camera = UnityEngine.Camera;

    public class PlayerFacade : Character
    {
        [SerializeField] private InputControlledBrain _inputControlledBrain;
        [SerializeField] private AIControlledBrain _aiControlledBrain;
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private Animator _animator;
        [SerializeField] private BallThrower _ballThrower;
        [SerializeField] private DistanceTracker _distanceTracker;

        private Ball _ball;
        private CinemachineVirtualCamera _virtualCamera;
        private PlayerStateMachine _stateMachine;
        private SceneConfig _sceneConfig;
        
        public PlayerStateMachine StateMachine => _stateMachine ??= new PlayerStateMachine(this); //TODO: methods?
        public Animator Animator => _animator;

        public event Action<bool> ThrowReached
        {
            add => _distanceTracker.ThrowReached += value;
            remove => _distanceTracker.ThrowReached -= value;
        }

        public event Action<bool> DunkReached
        {
            add => _distanceTracker.DunkReached += value;
            remove => _distanceTracker.DunkReached += value;
        }

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

        public void Initialize(Ball ball, Camera gameplayCamera, CinemachineVirtualCamera virtualCamera,
            SceneConfig sceneConfig)
        {
            _ballThrower.Initialize(ball, gameplayCamera);
            _inputControlledBrain.Initialize(gameplayCamera.transform);
            _distanceTracker.Initialize(sceneConfig.EnemyRing.transform.position);
            _ball = ball;
            _virtualCamera = virtualCamera;
            _virtualCamera.Follow = transform;
            _sceneConfig = sceneConfig;
        }

        public void PrioritizeCamera() =>
            _virtualCamera.Prioritize();

        public void DeprioritizeCamera() =>
            _virtualCamera.Deprioritize();
        
        public void FocusOnEnemyBasket() =>
            _virtualCamera.LookAt = _sceneConfig.EnemyRing.transform;

        public void FocusOnBall() =>
            _virtualCamera.LookAt = _ball.transform;
    }
}