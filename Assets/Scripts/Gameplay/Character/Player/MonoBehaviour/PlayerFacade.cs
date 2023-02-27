using System;
using Cinemachine;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Dunk;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Pass;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw;
using Gameplay.Character.Player.MonoBehaviour.Brains;
using Gameplay.Character.Player.MonoBehaviour.Distance;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using Gameplay.Character.Player.MonoBehaviour.TriggerZone;
using Gameplay.Character.Player.StateMachine;
using Gameplay.Character.Player.StateMachine.States;
using Infrastructure.Input.InputService;
using Scene.Ring;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour
{
    public class PlayerFacade : CharacterFacade
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
        [SerializeField] private FightForBallTriggerZone _fightForBallTriggerZone;

        private CinemachineVirtualCamera _virtualCamera;
        private PlayerStateMachine _stateMachine;
        private PlayerFacade _ally;
        private Ring _enemyRing;

        private PlayerStateMachine StateMachine => _stateMachine ??= new PlayerStateMachine(this);
        public Animator Animator => _animator;
        public bool IsPassPossible => _distanceTracker.IsPassPossible && OwnsBall;
        public bool IsInDunkZone => _distanceTracker.IsInDunkZone && OwnsBall;
        public bool IsInThrowZone => _distanceTracker.InThrowZone && OwnsBall;
        public bool IsPlayable { get; private set; }
        private Type CurrentState => StateMachine.CurrentState;

        public bool IsControlled => CurrentState == typeof(InputControlledAttackState) ||
                                    CurrentState == typeof(InputControlledDefenceState);

        public event Action<bool> ThrowReached
        {
            add => _distanceTracker.ThrowReached += value;
            remove => _distanceTracker.ThrowReached -= value;
        }

        public event Action<bool> DunkReached
        {
            add => _distanceTracker.DunkReached += value;
            remove => _distanceTracker.DunkReached -= value;
        }

        public event Action<bool> PassReached
        {
            add => _distanceTracker.PassReached += value;
            remove => _distanceTracker.PassReached -= value;
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

        public event Action<PlayerFacade[]> FightForBallStarted
        {
            add => _fightForBallTriggerZone.FightForBallStarted += value;
            remove => _fightForBallTriggerZone.FightForBallStarted -= value;
        }

        public PlayerFacade Initialize(bool isPlayable, PlayerFacade ally, Ball.MonoBehavior.Ball ball,
            UnityEngine.Camera gameplayCamera, CinemachineVirtualCamera virtualCamera, Ring enemyRing, IInputService inputService)
        {
            _enemyRing = enemyRing;
            IsPlayable = isPlayable;
            _dunker.Initialize(ball);
            _ally = ally;
            _inputControlledBrain.Initialize(inputService);
            _ballThrower.Initialize(ball, gameplayCamera, inputService);
            _inputControlledBrain.Initialize(gameplayCamera.transform);
            _distanceTracker.Initialize(enemyRing.transform.position, ally.transform);
            Ball = ball;
            _virtualCamera = virtualCamera;
            _virtualCamera.Follow = transform;
            _passer.Initialize(ball, _ally);

            return this;
        }

        public void EnterInputControlledAttackState() =>
            StateMachine.Enter<InputControlledAttackState>();

        public void EnterInputControlledDefenceState() =>
            StateMachine.Enter<InputControlledDefenceState>();

        public void EnterAIControlledState() =>
            StateMachine.Enter<AIControlledState>();

        public void EnterThrowState(Vector3 ringPosition) =>
            StateMachine.Enter<ThrowState, Vector3>(ringPosition);

        public void EnterIdleState() =>
            StateMachine.Enter<IdleState>();

        public void EnterPassState() =>
            StateMachine.Enter<PassState>();

        public void EnterCatchState() =>
            StateMachine.Enter<CatchState>();

        public void EnterDunkState(Ring ring) =>
            StateMachine.Enter<DunkState, Ring>(ring);

        public void EnterFightForBallState(PlayerFacade opponent) =>
            StateMachine.Enter<FightForBallState, PlayerFacade>(opponent);
        
        public void EnterNotControlledState() =>
            StateMachine.Enter<NotControlledState>();

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

        public void EnableFightForBallTriggerZone() =>
            _fightForBallTriggerZone.Enable();

        public void DisableBallContestTriggerZone() =>
            _fightForBallTriggerZone.Disable();

        public void PrioritizeCamera() =>
            _virtualCamera.Prioritize();

        public void FocusOnEnemyBasket() =>
            _virtualCamera.LookAt = _enemyRing.transform;

        public void FocusOnBall() =>
            _virtualCamera.LookAt = Ball.transform;

        public void FocusOnAlly() =>
            _virtualCamera.LookAt = _ally.transform;

        public void FocusOnBallOwner() =>
            _virtualCamera.LookAt = Ball.Owner.transform;

        public void FocusOn(Transform target) =>
            _virtualCamera.LookAt = target;

        public void RotateTo(Vector3 position, Action callback = null) =>
            _playerMover.RotateTo(position, callback);

        public void Dunk(Ring ring) =>
            _dunker.Dunk(ring);

        public void RotateToAlly(Action callback = null) =>
            _playerMover.RotateTo(_ally.transform.position, callback);

        public void Pass() =>
            _passer.Pass();

        public void RotateToBallOwner(Action callback = null) =>
            _playerMover.RotateTo(Ball.Owner.transform.position, callback);
    }
}