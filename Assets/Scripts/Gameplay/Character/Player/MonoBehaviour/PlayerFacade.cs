using System;
using System.Collections.Generic;
using Cinemachine;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Dunk;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Pass;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw;
using Gameplay.Character.Player.MonoBehaviour.Brains;
using Gameplay.Character.Player.MonoBehaviour.Brains.InputControlled;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using Gameplay.Character.Player.MonoBehaviour.TargetTracking;
using Gameplay.Character.Player.MonoBehaviour.TriggerZone;
using Gameplay.Character.Player.StateMachine;
using Gameplay.Character.Player.StateMachine.States;
using Infrastructure.Input.InputService;
using NodeCanvas.BehaviourTrees;
using Scene;
using Scene.Ring;
using UnityEngine;
using Utility.Constants;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour
{
    using Ball.MonoBehavior;

    public class PlayerFacade : CharacterFacade
    {
        [SerializeField] private InputControlledDefenceBrain _inputControlledDefenceBrain;
        [SerializeField] private InputControlledAttackBrain _inputControlledAttackBrain;
        [SerializeField] private AIControlledEventLauncher _aiControlledEventLauncher;
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private Animator _animator;
        [SerializeField] private BallThrower _ballThrower;
        [SerializeField] private TargetTracker _targetTracker;
        [SerializeField] private Passer _passer;
        [SerializeField] private Catcher _catcher;
        [SerializeField] private Dunker _dunker;
        [SerializeField] private FightForBallTriggerZone _fightForBallTriggerZone;
        [SerializeField] private BehaviourTreeOwner _defenceBehaviourTree;
        [SerializeField] private BehaviourTreeOwner _attackWithBallBehaviourTree;
        [SerializeField] private BehaviourTreeOwner _attackWithoutBallBehaviourTree;
        [SerializeField] private CharacterController _characterController;

        private CinemachineVirtualCamera _virtualCamera;
        private PlayerStateMachine _stateMachine;
        private PlayerFacade _ally;
        private Ring _oppositeRing;

        public void Initialize
        (
            bool isPlayable,
            PlayerFacade ally,
            Ball ball,
            PlayerFacade[] oppositeTeam,
            UnityEngine.Camera gameplayCamera,
            CinemachineVirtualCamera virtualCamera,
            SceneConfig sceneConfig,
            bool leftSide,
            IInputService inputService
        )
        {
            LeftSide = leftSide;
            IsPlayable = isPlayable;
            _oppositeRing = LeftSide ? sceneConfig.RightRing : sceneConfig.LeftRing;
            Ring playerRing = LeftSide ? sceneConfig.LeftRing : sceneConfig.RightRing;
            _dunker.Initialize(ball, _oppositeRing);
            _ally = ally;
            _inputControlledDefenceBrain.Initialize(gameplayCamera.transform, inputService);
            _inputControlledAttackBrain.Initialize(gameplayCamera.transform, inputService);
            _ballThrower.Initialize(ball, gameplayCamera, inputService);
            _targetTracker.Initialize(_oppositeRing.transform.position, ally.transform);
            Ball = ball;
            _virtualCamera = virtualCamera;
            _virtualCamera.Follow = transform;
            _passer.Initialize(ball, _ally);
            _fightForBallTriggerZone.Initialize(ally);

            _attackWithoutBallBehaviourTree.AddBinds(new Dictionary<string, object>
            {
                [BehaviourTreeVariableNames.AllyVariableName] = _ally,
                [BehaviourTreeVariableNames.OppositeRingVariableName] = _oppositeRing,
                [BehaviourTreeVariableNames.CourtDimensionsVariableName] = sceneConfig.CourtDimensions,
                [BehaviourTreeVariableNames.OppositeTeamVariableName] = oppositeTeam
            });

            _attackWithBallBehaviourTree.AddBinds(new Dictionary<string, object>
            {
                [BehaviourTreeVariableNames.AllyVariableName] = _ally,
                [BehaviourTreeVariableNames.OppositeRingVariableName] = _oppositeRing,
                [BehaviourTreeVariableNames.CourtDimensionsVariableName] = sceneConfig.CourtDimensions,
                [BehaviourTreeVariableNames.OppositeTeamVariableName] = oppositeTeam
            });

            _defenceBehaviourTree.AddBinds(new Dictionary<string, object>
            {
                [BehaviourTreeVariableNames.AllyVariableName] = _ally,
                [BehaviourTreeVariableNames.PlayerRingVariableName] = playerRing,
                [BehaviourTreeVariableNames.CourtDimensionsVariableName] = sceneConfig.CourtDimensions,
                [BehaviourTreeVariableNames.OppositeTeamVariableName] = oppositeTeam
            });
        }

        public void Configure(float speed)
        {
            _playerMover.Configure(speed);
        }

        private PlayerStateMachine StateMachine => _stateMachine ??= new PlayerStateMachine(this);
        private Type CurrentState => StateMachine.CurrentState;
        public Animator Animator => _animator;
        public bool IsPlayable { get; private set; }
        public bool LeftSide { get; private set; }

        public bool IsControlled => CurrentState == typeof(InputControlledAttackState) ||
                                    CurrentState == typeof(InputControlledDefenceState);

        public Ring OppositeRing => _oppositeRing;
        public float MaxPassDistance => _targetTracker.MaxPassDistance;
        public bool IsInPassDistance => _targetTracker.IsInPassDistance;
        public bool IsInThrowDistance => _targetTracker.IsInThrowDistance;
        public bool IsInDunkDistance => _targetTracker.IsInDunkDistance;

        public event Action ChangePlayerInitiated
        {
            add => _inputControlledDefenceBrain.ChangePlayerInitiated += value;
            remove => _inputControlledDefenceBrain.ChangePlayerInitiated -= value;
        }

        public event Action<PlayerFacade> PassInitiated
        {
            add
            {
                _aiControlledEventLauncher.PassInitiated += value;
                _inputControlledAttackBrain.PassInitiated += value;
            }
            remove
            {
                _aiControlledEventLauncher.PassInitiated -= value;
                _inputControlledAttackBrain.PassInitiated -= value;
            }
        }

        public event Action<PlayerFacade> ThrowInitiated
        {
            add
            {
                _inputControlledAttackBrain.ThrowInitiated += value;
                _aiControlledEventLauncher.ThrowInitiated += value;
            }
            remove
            {
                _inputControlledAttackBrain.ThrowInitiated -= value;
                _aiControlledEventLauncher.ThrowInitiated -= value;
            }
        }

        public event Action<PlayerFacade> DunkInitiated
        {
            add
            {
                _inputControlledAttackBrain.DunkInitiated += value;
                _aiControlledEventLauncher.DunkInitiated += value;
            }
            remove
            {
                _inputControlledAttackBrain.DunkInitiated -= value;
                _aiControlledEventLauncher.DunkInitiated -= value;
            }
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

        public void EnterThrowState() =>
            StateMachine.Enter<ThrowState>();

        public void EnterIdleState() =>
            StateMachine.Enter<IdleState>();

        public void EnterPassState() =>
            StateMachine.Enter<PassState>();

        public void EnterCatchState(PlayerFacade passingPlayer) =>
            StateMachine.Enter<CatchState, PlayerFacade>(passingPlayer);

        public void EnterDunkState() =>
            StateMachine.Enter<DunkState>();

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
            _attackWithoutBallBehaviourTree.Enable();

        public void DisableAIControlledAttackWithoutBallBrain() =>
            _attackWithoutBallBehaviourTree.Disable();

        public void EnableAIControlledAttackWithBallBrain() =>
            _attackWithBallBehaviourTree.Enable();

        public void DisableAIControlledAttackWithBallBrain() =>
            _attackWithBallBehaviourTree.Disable();

        public void EnableAIControlledDefenceBrain() =>
            _defenceBehaviourTree.Enable();

        public void DisableAIControlledDefenceBrain() =>
            _defenceBehaviourTree.Disable();

        public void EnablePlayerMover() =>
            _playerMover.Enable();

        public void DisablePlayerMover() =>
            _playerMover.Disable();

        public void EnableBallThrower() =>
            _ballThrower.Enable();

        public void DisableBallThrower() =>
            _ballThrower.Disable();

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

        public void EnableCharacterController() =>
            _characterController.Enable();

        public void DisableCharacterController() =>
            _characterController.Disable();

        public void PrioritizeCamera() =>
            _virtualCamera.Prioritize();

        public void FocusOnOppositeBasket() =>
            _virtualCamera.LookAt = _oppositeRing.transform;

        public void FocusOnBall() =>
            _virtualCamera.LookAt = Ball.transform;

        public void FocusOnAlly() =>
            _virtualCamera.LookAt = _ally.transform;

        public void FocusOnBallOwner() =>
            _virtualCamera.LookAt = Ball.Owner.transform;

        public void FocusOn(Transform target) =>
            _virtualCamera.LookAt = target;

        public void RotateTo(Vector3 position, Action todoAfter = null) =>
            _playerMover.RotateTo(position, todoAfter);

        public void RotateToAlly(Action todoAfter = null) =>
            _playerMover.RotateTo(_ally.transform.position, todoAfter);

        public void RotateToBallOwner(Action todoAfter = null) =>
            _playerMover.RotateTo(Ball.Owner.transform.position, todoAfter);

        public void RotateToRing(Action todoAfter = null) =>
            _playerMover.RotateTo(_oppositeRing.transform.position, todoAfter);

        public void Pass() =>
            _passer.Pass();

        public void Dunk() =>
            _dunker.Dunk();
    }
}