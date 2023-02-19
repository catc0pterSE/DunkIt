using Gameplay.Character;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.Transitions;
using Infrastructure.CoroutineRunner;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;
using Scene;
using UI;
using UI.HUD;
using UI.HUD.StateMachine.States;
using Utility.Constants;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.Gameplay
{
    public class GameplayState : StateWithTransitions, IParameterlessState
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly PlayerFacade[] _enemyTeam;
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly IGameplayHUD _gameplayHud;
        private PlayerFacade _controlledPlayer;
        private IInputService _inputService;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();
        private PlayerFacade NotControlledPlayer => _playerTeam.FindFirstOrNull(player => player != _controlledPlayer);
        public PlayerFacade ControlledPlayer => _controlledPlayer;

        public GameplayState
        (
            PlayerFacade[] playerTeam,
            PlayerFacade[] enemyTeam,
            Ball.MonoBehavior.Ball ball,
            SceneConfig sceneConfig,
            IGameplayHUD gameplayHud,
            GameplayLoopStateMachine gameplayLoopStateMachine,
            LoadingCurtain loadingCurtain,
            ICoroutineRunner coroutineRunner
        )
        {
            Transitions = new ITransition[]
            {
                new GameplayStateToDunkStateTransition(this, gameplayLoopStateMachine),
                new GameplayStateToThrowStateTransition(this, gameplayLoopStateMachine),
                new AnyToFightForBallTransition(ball, gameplayLoopStateMachine, coroutineRunner, true),
                new GameplayStateToUpsetCutsceneStateTransition(playerTeam, enemyTeam, ball, sceneConfig.EnemyRing,
                    loadingCurtain, gameplayLoopStateMachine, coroutineRunner, sceneConfig),
                new GameplayStateToPassTransition(this, gameplayLoopStateMachine)
            };
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _ball = ball;
            _gameplayHud = gameplayHud;
        }

        public override void Enter()
        {
            base.Enter();
            EnableHUD();
            SubscribeOnChangePlayerInput();
            SetControlledPlayer
                (
                    _playerTeam.FindFirstOrNull(player => player.OwnsBall)
                    ?? _playerTeam[NumericConstants.PrimaryTeamMemberIndex]
                );
            SubscribeOnBall();
            SetPlayersStates();
            SetEnemiesStates();
            SetHUDState();
        }

        public override void Exit()
        {
            base.Exit();
            DisableHUD();
            UnsubscribeOfChangePlayerInput();
            UnsubscribeFromBall();
        }

        private void SetEnemiesStates() =>
            _enemyTeam.Map(enemy =>
                enemy.EnterAIControlledState());

        private void EnableHUD() =>
            _gameplayHud.Enable();

        private void DisableHUD() =>
            _gameplayHud.Disable();

        private void SubscribeOnBall() =>
            _ball.OwnerChanged += OnBallOwnerChanged;

        private void UnsubscribeFromBall() =>
            _ball.OwnerChanged -= OnBallOwnerChanged;

        private void OnBallOwnerChanged(CharacterFacade newOwner)
        {
            if (newOwner is PlayerFacade player)
                SetControlledPlayer(player);

            SetPlayersStates();
            SetHUDState();
        }

        private void SetPlayersStates()
        {
            if (ControlledPlayer.OwnsBall)
                _controlledPlayer.EnterInputControlledAttackState();
            else
                _controlledPlayer.EnterInputControlledDefenceState();

            NotControlledPlayer.EnterAIControlledState();
        }

        private void SetHUDState()
        {
            if (ControlledPlayer.OwnsBall)
                _gameplayHud.StateMachine.Enter<AttackState, PlayerFacade>(_controlledPlayer);
            else
                _gameplayHud.StateMachine.Enter<DefenceState>();
        }

        private void SetControlledPlayer(PlayerFacade player) =>
            _controlledPlayer = player;

        private void SubscribeOnChangePlayerInput() =>
            InputService.ChangePlayerButtonDown += SwapControlledPlayer;

        private void UnsubscribeOfChangePlayerInput() =>
            InputService.ChangePlayerButtonDown -= SwapControlledPlayer;

        private void SwapControlledPlayer()
        {
            if (ControlledPlayer.OwnsBall)
                return;

            SetControlledPlayer(NotControlledPlayer);
            SetPlayersStates();
            SetHUDState();
        }
    }
}