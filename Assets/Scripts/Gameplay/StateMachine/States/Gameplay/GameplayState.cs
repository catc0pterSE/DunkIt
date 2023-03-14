using System.Linq;
using Gameplay.Character;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.Transitions;
using Infrastructure.CoroutineRunner;
using Modules.StateMachine;
using Scene;
using Scene.Ring;
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
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _ball = ball;
            _gameplayHud = gameplayHud;
            Ring enemyRing = _playerTeam.First().OppositeRing;

            Transitions = new ITransition[]
            {
                new GameplayStateToDunkStateTransition(gameplayLoopStateMachine, playerTeam),
                new GameplayStateToThrowStateTransition(playerTeam, gameplayLoopStateMachine),
                new AnyToFightForBallTransition(ball, gameplayLoopStateMachine, coroutineRunner, true),
                new GameplayStateToUpsetCutsceneStateTransition(enemyRing, gameplayLoopStateMachine),
                new GameplayStateToPassTransition(playerTeam, enemyTeam, gameplayLoopStateMachine),
                new AnyToDropBallTransition(ball, gameplayLoopStateMachine, loadingCurtain, playerTeam, enemyTeam, sceneConfig)
            };
           
        }

        public override void Enter()
        {
            base.Enter();
            EnableHUD();
            SubscribeOnPlayerTeam();
            SetControlledPlayer
            (
                _playerTeam.FindFirstOrNull(player => player.OwnsBall)
                ?? _playerTeam[NumericConstants.PrimaryTeamMemberIndex]
            );
            OnBallOwnerChanged(_ball.Owner);
            SubscribeOnBall();
        }

        public override void Exit()
        {
            base.Exit();
            DisableHUD();
            UnsubscribeFromPlayerTeam();
            UnsubscribeFromBall();
        }

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
            if (newOwner is not PlayerFacade player)
                return;

            SetPlayersStates(player);
            SetHUDState();
        }

        private void SetPlayersStates(PlayerFacade ballOwner)
        {
            if (ballOwner.IsPlayable)
            {
                SetControlledPlayer(ballOwner);
                SetPlayerTeamAttackStates();
                SetEnemyTeamDefenceStates();
            }
            else
            {
                SetEnemyTeamAttackStates(ballOwner);
                SetPlayerTeamDefenceStates();
            }
        }

        private void SetPlayerTeamAttackStates()
        {
            _controlledPlayer.EnterInputControlledAttackState();
            NotControlledPlayer.EnterAIControlledAttackWithoutBallState();
        }

        private void SetEnemyTeamDefenceStates() =>
            _enemyTeam.Map(enemyPlayer => enemyPlayer.EnterAIControlledDefenceState());

        private void SetEnemyTeamAttackStates(PlayerFacade ballOwner)
        {
            PlayerFacade enemyWithBall = _enemyTeam.FindFirstOrNull(enemyPlayer => enemyPlayer == ballOwner);
            enemyWithBall.EnterAIControlledAttackWithBallState();
            PlayerFacade enemyWithoutBall = _enemyTeam.FindFirstOrNull(enemyPlayer => enemyPlayer != ballOwner);
            enemyWithoutBall.EnterAIControlledAttackWithoutBallState();
        }

        private void SetPlayerTeamDefenceStates()
        {
            ControlledPlayer.EnterInputControlledDefenceState();
            NotControlledPlayer.EnterAIControlledDefenceState();
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


        private void SubscribeOnPlayerTeam() =>
            _playerTeam.Map(player => player.ChangePlayerInitiated += SwapControlledPlayer);

        private void UnsubscribeFromPlayerTeam() =>
            _playerTeam.Map(player => player.ChangePlayerInitiated -= SwapControlledPlayer);

        private void SwapControlledPlayer()
        {
            SetControlledPlayer(NotControlledPlayer);
            SetPlayerTeamDefenceStates();
            SetHUDState();
        }
    }
}