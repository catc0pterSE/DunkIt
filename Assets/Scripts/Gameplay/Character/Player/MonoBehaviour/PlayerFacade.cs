using System;
using Cinemachine;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Dunk;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Pass;
using Gameplay.Character.Player.MonoBehaviour.BallHandle.Throw;
using Gameplay.Character.Player.MonoBehaviour.Brains.AIControlled;
using Gameplay.Character.Player.MonoBehaviour.Brains.LocalControlled;
using Gameplay.Character.Player.MonoBehaviour.Events;
using Gameplay.Character.Player.MonoBehaviour.Movement;
using Gameplay.Character.Player.MonoBehaviour.TargetTracking;
using Gameplay.Character.Player.MonoBehaviour.TriggerZone;
using Gameplay.Character.Player.StateMachine;
using Gameplay.Character.Player.StateMachine.States;
using Infrastructure.Input.InputService;
using Infrastructure.Mediator;
using Infrastructure.PlayerService;
using Scene;
using Scene.Ring;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour
{
    public class PlayerFacade : CharacterFacade
    {
        [SerializeField] private PlayerEventLauncher _playerEventLauncher;
        [SerializeField] private AIControlledBrain _aiControlledBrain;
        [SerializeField] private LocalControlledBrain _localControlledBrain;
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private Animator _animator;
        [SerializeField] private BallThrower _ballThrower;
        [SerializeField] private LocalControlledThrowing _localControlledThrowing;
        [SerializeField] private AIControlledThrowing _aiControlledThrowing;
        [SerializeField] private TargetTracker _targetTracker;
        [SerializeField] private Passer _passer;
        [SerializeField] private Catcher _catcher;
        [SerializeField] private Dunker _dunker;
        [SerializeField] private FightForBallTriggerZone _fightForBallTriggerZone;

        private TeamsMediator _teamsMediator;
        private CinemachineVirtualCamera _virtualCamera;
        private PlayerStateMachine _stateMachine;

        public void Initialize
        (
            TeamsMediator teamsMediator,
            bool canBeLocalControlled,
            bool isAIOnlyControlled,
            CinemachineVirtualCamera virtualCamera
        )
        {
            CanBeLocalControlled = canBeLocalControlled;
            IsAIOnlyControlled = isAIOnlyControlled;
            _teamsMediator = teamsMediator;
            _virtualCamera = virtualCamera;
            Ball = _teamsMediator.GetBall();

            IPlayerService playerService = _teamsMediator.GetPlayerService();
            _stateMachine = new PlayerStateMachine(this, Ball, playerService);

            UnityEngine.Camera gameplayCamera = _teamsMediator.GetCamera();
            IInputService inputService = teamsMediator.GetInputService();
            PlayerFacade[] allies = teamsMediator.GetAllies(this);
            PlayerFacade[] oppositeTeam = teamsMediator.GetOppositeTeamPlayers(this);
            Ring playerRing = teamsMediator.GetPlayersRing(this);
            OppositeRing = teamsMediator.GetOppositeRing(this);
            CourtDimensions courtDimensions = teamsMediator.GetCourtDimensions();

            _playerEventLauncher.Initialize(teamsMediator);
            _dunker.Initialize(Ball, OppositeRing);
            _localControlledBrain.Initialize(gameplayCamera.transform, inputService);
            _ballThrower.Initialize(Ball);
            _aiControlledThrowing.Initialize(OppositeRing);
            _localControlledThrowing.Initialize(gameplayCamera, inputService, OppositeRing);
            _targetTracker.Initialize(OppositeRing.transform.position, allies);
            _virtualCamera.Follow = transform;
            _passer.Initialize(Ball);
            _fightForBallTriggerZone.Initialize(allies);
            _aiControlledBrain.Initialize(allies, OppositeRing, playerRing, courtDimensions, oppositeTeam);
        }

        public void Configure(float speed)
        {
            _playerMover.Configure(speed);
        }

        public Animator Animator => _animator;
        public bool CanBeLocalControlled { get; private set; }
        public Ring OppositeRing { get; private set; }

        public float MaxPassDistance => _targetTracker.MaxPassDistance;
        public bool IsInThrowDistance => _targetTracker.IsInThrowDistance;
        public bool IsInDunkDistance => _targetTracker.IsInDunkDistance;
        public bool IsAIOnlyControlled { get; private set; }

        public Type CurrentStateType => _stateMachine.CurrentState;

        public event Action<PlayerFacade, PlayerFacade> PassInitiated
        {
            add => _playerEventLauncher.PassInitiated += value;
            remove => _playerEventLauncher.PassInitiated -= value;
        }

        public event Action<PlayerFacade> ThrowInitiated
        {
            add => _playerEventLauncher.ThrowInitiated += value;
            remove => _playerEventLauncher.ThrowInitiated -= value;
        }

        public event Action<PlayerFacade> DunkInitiated
        {
            add => _playerEventLauncher.DunkInitiated += value;
            remove => _playerEventLauncher.DunkInitiated -= value;
        }

        public event Action<PlayerFacade> ChangePlayerInitiated
        {
            add => _playerEventLauncher.ChangePlayerInitiated += value;
            remove => _playerEventLauncher.ChangePlayerInitiated -= value;
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

        public void EnterThrowState() =>
            _stateMachine.Enter<ThrowState>();

        public void EnterIdleState(Vector3 lookingAt) =>
            _stateMachine.Enter<IdleState, Vector3>(lookingAt);

        public void EnterDropBallState() =>
            _stateMachine.Enter<DropBallState>();

        public void EnterPassState(PlayerFacade passTarget) =>
            _stateMachine.Enter<PassState, PlayerFacade>(passTarget);

        public void EnterDunkState() =>
            _stateMachine.Enter<DunkState>();

        public void EnterFightForBallState(PlayerFacade opponent) =>
            _stateMachine.Enter<FightForBallState, PlayerFacade>(opponent);

        public void EnterNotControlledState() =>
            _stateMachine.Enter<NotControlledState>();

        public void EnterAttackWithBallState() =>
            _stateMachine.Enter<AttackWithBallState>();

        public void EnterAttackWithoutBallState() =>
            _stateMachine.Enter<AttackWithoutBallState>();

        public void EnterDefenceState() =>
            _stateMachine.Enter<DefenceState>();

        public void EnterBallChasingState() =>
            _stateMachine.Enter<BallChasingState>();

        public void EnableLocalControlledBrain(LocalAction[] localActions) =>
            _localControlledBrain.Enable(localActions);

        public void DisableLocalControlledBrain() =>
            _localControlledBrain.Disable();

       public void EnableAIControlledBrain(AI aiType) =>
            _aiControlledBrain.Enable(aiType);

        public void DisableAIControlledBrain() =>
            _aiControlledBrain.Disable();

        public void EnableLocalControlledBallThrower() =>
            _localControlledThrowing.Enable();

        public void DisableLocalControlledBallThrower() =>
            _localControlledThrowing.Disable();

        public void EnableAIControlledBallThrower() =>
            _aiControlledThrowing.Enable();

        public void DisableAIControlledBallThrower() =>
            _aiControlledThrowing.Disable();

        public void EnableTargetTracker() =>
            _targetTracker.Enable();

        public void DisableTargetTracker() =>
            _targetTracker.Disable();

        public void EnableCatcher() =>
            _catcher.Enable();

        public void DisableCatcher() =>
            _catcher.Disable();

        public void EnableFightForBallTriggerZone() =>
            _fightForBallTriggerZone.Enable();

        public void DisableFightForBallTriggerZone() =>
            _fightForBallTriggerZone.Disable();

        public void EnableMover() =>
            _playerMover.Enable();

        public void DisableMover() =>
            _playerMover.Disable();

        public void PrioritizeCamera() =>
            _virtualCamera.Prioritize();

        public void FocusOn(Transform target) =>
            _virtualCamera.LookAt = target;

        public void RotateTo(Vector3 position, Action todoAfter = null) =>
            _playerMover.RotateTo(position, todoAfter);

        public void Pass(PlayerFacade to) =>
            _passer.Pass(to);

        public void Dunk() =>
            _dunker.Dunk();
    }
}