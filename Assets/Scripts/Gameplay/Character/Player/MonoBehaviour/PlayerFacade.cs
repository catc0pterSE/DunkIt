using System;
using Cinemachine;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw;
using Gameplay.Character.Player.MonoBehaviour.Brains;
using Gameplay.Character.Player.MonoBehaviour.Distance;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using Gameplay.Character.Player.StateMachine;
using Scene;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour
{
    public class PlayerFacade : BasketballPlayer
    {
        [SerializeField] private InputControlledBrain _inputControlledBrain;
        [SerializeField] private AIControlledBrain _aiControlledBrain;
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private Animator _animator;
        [SerializeField] private BallThrower _ballThrower;
        [SerializeField] private DistanceTracker _distanceTracker;

        private Ball.MonoBehavior.Ball _ball;
        private CinemachineVirtualCamera _virtualCamera;
        private PlayerStateMachine _stateMachine;
        private SceneConfig _sceneConfig;
        
        public PlayerStateMachine StateMachine => _stateMachine ??= new PlayerStateMachine(this); //TODO: methods?
        public Animator Animator => _animator;
        public bool IsInDunkZone => _distanceTracker.IsInDunkZone;
        public bool IsInThrowZone => _distanceTracker.InThrowZone;

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

        public override event Action BallThrown
        {
            add => _ballThrower.BallThrown += value;
            remove => _ballThrower.BallThrown -= value;
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

        public void EnableBallThrower() =>
            _ballThrower.Enable();

        public void DisableBallThrower() =>
            _ballThrower.Disable();

        public void EnableDistanceTracker() =>
            _distanceTracker.Enable();

        public void DisableDistanceTracker() =>
            _distanceTracker.Disable();
        

        public void Initialize(PlayerFacade ally, Ball.MonoBehavior.Ball ball, UnityEngine.Camera gameplayCamera, CinemachineVirtualCamera virtualCamera,
            SceneConfig sceneConfig)
        {
            _ballThrower.Initialize(ball, gameplayCamera);
            _inputControlledBrain.Initialize(gameplayCamera.transform);
            _distanceTracker.Initialize(sceneConfig.EnemyRing.transform.position, ally.transform);
            _ball = ball;
            _virtualCamera = virtualCamera;
            _virtualCamera.Follow = transform;
            _sceneConfig = sceneConfig;
        }

        public void PrioritizeCamera() =>
            _virtualCamera.Prioritize();
        
        public void FocusOnEnemyBasket() =>
            _virtualCamera.LookAt = _sceneConfig.EnemyRing.transform;

        public void FocusOnBall() =>
            _virtualCamera.LookAt = _ball.transform;
    }
}