using System;
using System.Collections.Generic;
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
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using Scene.Ring;
using UnityEngine;
using Utility.Extensions;
using Object = UnityEngine.Object;

namespace Gameplay.Character.Player.MonoBehaviour
{
    public class PlayerFacade : CharacterFacade
    {
        [SerializeField] private InputControlledDefenceBrain _inputControlledDefenceBrain;
        [SerializeField] private InputControlledAttackBrain _inputControlledAttackBrain;
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private Animator _animator;
        [SerializeField] private InputBallThrower _inputBallThrower;
        [SerializeField] private TargetTracker _targetTracker;
        [SerializeField] private Passer _passer;
        [SerializeField] private Catcher _catcher;
        [SerializeField] private Dunker _dunker;
        [SerializeField] private FightForBallTriggerZone _fightForBallTriggerZone;
        [SerializeField] private BehaviourTreeOwner _defenceBehaviourTree;
        [SerializeField] private BehaviourTreeOwner _attackWithBallBehaviourTree;
        [SerializeField] private BehaviourTreeOwner _attackWithoutBallBehaviourTree;
        [SerializeField] private Blackboard _behaviorTreeBlackboard;

        private CinemachineVirtualCamera _virtualCamera;
        private PlayerStateMachine _stateMachine;
        private PlayerFacade _ally;
        private Ring _enemyRing;

        public PlayerFacade Initialize(bool isPlayable, PlayerFacade ally, Ball.MonoBehavior.Ball ball,
            UnityEngine.Camera gameplayCamera, CinemachineVirtualCamera virtualCamera, Ring enemyRing,
            IInputService inputService)
        {
            _enemyRing = enemyRing;
            IsPlayable = isPlayable;
            _dunker.Initialize(ball);
            _ally = ally;
            _inputControlledDefenceBrain.Initialize(gameplayCamera.transform, inputService);
            _inputControlledAttackBrain.Initialize(gameplayCamera.transform, inputService);
            _inputBallThrower.Initialize(ball, gameplayCamera, inputService);
            _targetTracker.Initialize(enemyRing.transform.position, ally.transform);
            Ball = ball;
            _virtualCamera = virtualCamera;
            _virtualCamera.Follow = transform;
            _passer.Initialize(ball, _ally);
            
            _attackWithoutBallBehaviourTree.AddBinds(new Dictionary<string, Object> //TODO: constants
            {
                ["Ally"] = _ally,
                ["EnemyRing"] = _enemyRing
            });

            return this;
        }

        private PlayerStateMachine StateMachine => _stateMachine ??= new PlayerStateMachine(this);
        private Type CurrentState => StateMachine.CurrentState;
        public Animator Animator => _animator;
        public bool IsPlayable { get; private set; }

        public bool IsControlled => CurrentState == typeof(InputControlledAttackState) ||
                                    CurrentState == typeof(InputControlledDefenceState);
        public float MaxPassDistance => _targetTracker.MaxPassDistance;

        public event Action ChangePlayerInitiated
        {
            add => _inputControlledDefenceBrain.ChangePlayerInitiated += value;
            remove => _inputControlledDefenceBrain.ChangePlayerInitiated -= value;
        }

        public event Action<PlayerFacade> PassInitiated
        {
            add => _inputControlledAttackBrain.PassInitiated += value;
            remove => _inputControlledAttackBrain.PassInitiated -= value;
        }

        public event Action<PlayerFacade> ThrowInitiated
        {
            add => _inputControlledAttackBrain.ThrowInitiated += value;
            remove => _inputControlledAttackBrain.ThrowInitiated -= value;
        }

        public event Action<PlayerFacade> DunkInitiated
        {
            add => _inputControlledAttackBrain.DunkInitiated += value;
            remove => _inputControlledAttackBrain.DunkInitiated -= value;
        }

        public event Action<bool> ThrowReached
        {
            add => _targetTracker.ThrowReached += value;
            remove => _targetTracker.ThrowReached -= value;
        }

        public event Action<bool> DunkReached
        {
            add => _targetTracker.DunkReached += value;
            remove => _targetTracker.DunkReached -= value;
        }

        public event Action<bool> PassReached
        {
            add => _targetTracker.PassReached += value;
            remove => _targetTracker.PassReached -= value;
        }

        public event Action BallThrown
        {
            add => _inputBallThrower.BallThrown += value;
            remove => _inputBallThrower.BallThrown -= value;
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

        public void EnterInputControlledAttackState() =>
            StateMachine.Enter<InputControlledAttackState>();

        public void EnterInputControlledDefenceState() =>
            StateMachine.Enter<InputControlledDefenceState>();

        public void EnterAIControlledAttackWithBallState() =>
            StateMachine.Enter<AIControlledAttackWithBallState>();

        public void EnterAIControlledAttackWithoutBallState() =>
            StateMachine.Enter<AIControlledAttackWithoutBallState>();

        public void EnterAIControlledDefenceState() =>
            StateMachine.Enter<AIControlledDefenceState>();

        public void EnterThrowState(Vector3 ringPosition) =>
            StateMachine.Enter<ThrowState, Vector3>(ringPosition);

        public void EnterIdleState() =>
            StateMachine.Enter<IdleState>();

        public void EnterPassState() =>
            StateMachine.Enter<PassState>();

        public void EnterCatchState(PlayerFacade passingPlayer) =>
            StateMachine.Enter<CatchState, PlayerFacade>(passingPlayer);

        public void EnterDunkState(Ring ring) =>
            StateMachine.Enter<DunkState, Ring>(ring);

        public void EnterFightForBallState(PlayerFacade opponent) =>
            StateMachine.Enter<FightForBallState, PlayerFacade>(opponent);

        public void EnterNotControlledState() =>
            StateMachine.Enter<NotControlledState>();

        public void EnableInputControlledDefenceBrain() =>
            _inputControlledDefenceBrain.Enable();

        public void DisableInputControlledDefenceBrain() =>
            _inputControlledDefenceBrain.Disable();

        public void EnableInputControlledAttackBrain() =>
            _inputControlledAttackBrain.Enable();

        public void DisableInputControlledAttackBrain() =>
            _inputControlledAttackBrain.Disable();

        public void EnableAIControlledAttackWithoutBallBrain() =>
            _attackWithoutBallBehaviourTree.enabled = true;

        public void DisableAIControlledAttackWithoutBallBrain() =>
            _attackWithoutBallBehaviourTree.enabled = false;

        public void EnablePlayerMover() =>
            _playerMover.Enable();

        public void DisablePlayerMover() =>
            _playerMover.Disable();

        public void EnableBallThrower() =>
            _inputBallThrower.Enable();

        public void DisableBallThrower() =>
            _inputBallThrower.Disable();

        public void EnableDistanceTracker() =>
            _targetTracker.Enable();

        public void DisableDistanceTracker() =>
            _targetTracker.Disable();

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

        public void DisableFightForBallTriggerZone() =>
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