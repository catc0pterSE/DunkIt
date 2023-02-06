using System;
using Cinemachine;
using Gameplay.Character.Player.MonoBehaviour.BallHandle;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Dunk;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Pass;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw;
using Gameplay.Character.Player.MonoBehaviour.Brains;
using Gameplay.Character.Player.MonoBehaviour.Distance;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using Gameplay.Character.Player.MonoBehaviour.TriggerZone;
using Gameplay.Character.Player.StateMachine;
using Scene;
using Scene.Ring;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour
{
    public class PlayerFacade : BasketballPlayerFacade
    {
        [SerializeField] private InputControlledBrain _inputControlledBrain;
        [SerializeField] private AIControlledBrain _aiControlledBrain;
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private Animator _animator;
        [SerializeField] private BallThrower _ballThrower;
        [SerializeField] private DistanceTracker _distanceTracker;
        [SerializeField] private Passer _passer;
        [SerializeField] private Catcher _catcher;
        [SerializeField] private Dunker _dunker;

        private CinemachineVirtualCamera _virtualCamera;
        private PlayerStateMachine _stateMachine;
        private SceneConfig _sceneConfig;
        private PlayerFacade _ally;
        
        public PlayerStateMachine StateMachine => _stateMachine ??= new PlayerStateMachine(this); //TODO: methods?
        public Animator Animator => _animator;

        public bool IsPassPossible => _distanceTracker.IsPassPossible && OwnsBall;
        public bool IsInDunkZone => _distanceTracker.IsInDunkZone && OwnsBall;
        public bool IsInThrowZone => _distanceTracker.InThrowZone && OwnsBall;

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
        
        public event Action<bool> PassReached
        {
            add => _distanceTracker.PassReached += value;
            remove => _distanceTracker.PassReached += value;
        }

        public event Action BallThrown
        {
            add => _ballThrower.BallThrown += value;
            remove => _ballThrower.BallThrown -= value;
        }
        
        public event Action PassedBall
        {
            add => _passer.PassedBall += value;
            remove => _passer.PassedBall -= value;
        }
        
        public event Action CaughtBall
        {
            add => _catcher.CaughtBall += value;
            remove => _catcher.CaughtBall -= value;
        }
        
        public event Action DunkPointReached
        {
            add => _dunker.DunkPointReached += value;
            remove => _dunker.DunkPointReached -= value;
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

        public void EnablePasser() =>
            _passer.Enable();

        public void DisablePasser() =>
            _passer.Disable();
        
        public void EnableCatcher() =>
            _catcher.Enable();

        public void DisableCatcher() =>
            _catcher.Disable();

        public void EnableDunker() =>
            _dunker.Enable();
        
        public void DisableDunker() =>
            _dunker.Disable();

        public void Initialize(PlayerFacade ally, Ball.MonoBehavior.Ball ball, UnityEngine.Camera gameplayCamera, CinemachineVirtualCamera virtualCamera,
            SceneConfig sceneConfig)
        {
            _ally = ally;
            _ballThrower.Initialize(ball, gameplayCamera);
            _inputControlledBrain.Initialize(gameplayCamera.transform);
            _distanceTracker.Initialize(sceneConfig.EnemyRing.transform.position, ally.transform);
            Ball = ball;
            _virtualCamera = virtualCamera;
            _virtualCamera.Follow = transform;
            _sceneConfig = sceneConfig;
            _passer.Initialize(ball, _ally);
        }

        public void PrioritizeCamera() =>
            _virtualCamera.Prioritize();
        
        public void FocusOnEnemyBasket() =>
            _virtualCamera.LookAt = _sceneConfig.EnemyRing.transform;

        public void FocusOnBall() =>
            _virtualCamera.LookAt = Ball.transform;
        
        public void FocusOnAlly() =>
            _virtualCamera.LookAt = _ally.transform;
        
        public void FocusOnBallOwner() =>
            _virtualCamera.LookAt = Ball.Owner.transform;

        public void RotateTo(Vector3 position, Action callback = null) =>
            _playerMover.RotateTo(position, callback);
        
        public void Dunk(Ring ring) =>
            _dunker.Dunk(ring);
        
        public void RotateToAlly( Action callback = null) =>
            _playerMover.RotateTo(_ally.transform.position, callback);

        public void Pass() =>
            _passer.Pass();
    }
}