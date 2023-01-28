using System.Linq;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.StateMachine.States;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.StateMachine;
using UI.HUD;
using UI.HUD.StateMachine.States;
using Utility.Constants;

namespace Gameplay.StateMachine.States.Gameplay
{
    public class GameplayState : StateWithTransitions
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly Ball.MonoBehavior.Ball _ball;
        private readonly IGameplayHUD _gameplayHud;
        private PlayerFacade _controlledPlayer;
        private IInputService _inputService;

        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();
        private bool ControlledPlayerIsInAttack => _ball.OwnerData.Value == _controlledPlayer;
        private PlayerFacade NotControlledPlayer => _playerTeam.FirstOrDefault(player => player != _controlledPlayer);

        public PlayerFacade ControlledPlayer => _controlledPlayer;

        public GameplayState(PlayerFacade[] playerTeam, Ball.MonoBehavior.Ball ball, IGameplayHUD gameplayHud) :
            base(new ITransition[]
                {
                }
            )
        {
            _playerTeam = playerTeam;
            _ball = ball;
            _gameplayHud = gameplayHud;
        }

        public override void Enter()
        {
            base.Enter();
            _gameplayHud.Enable();
            ObserveBall();
            SubscribeOnChangePlayerInput();
        }

        public override void Exit()
        {
            base.Exit();
            _gameplayHud.Disable();
            StopObserveBall();
            UnsubscribeOnChangePlayerInput();
        }

        private void ObserveBall() =>
            _ball.OwnerData.Observe(OnBallOwnerChanged);


        private void StopObserveBall() =>
            _ball.OwnerData.Dispose();

        private void OnBallOwnerChanged(Character.Character newOwner)
        {
            if (newOwner is PlayerFacade player)
                SetCurrentPlayer(player);

            SetPlayersState();
            SetHUDState();
        }

        private void SetPlayersState()
        {
            if (_controlledPlayer == null)
                SetCurrentPlayer(_playerTeam[NumericConstants.PrimaryTeamMemberIndex]);

            if (ControlledPlayerIsInAttack)
                _controlledPlayer.StateMachine.Enter<ControlledAttackState>();
            else
                _controlledPlayer.StateMachine.Enter<ControlledDefenceState>();

            NotControlledPlayer.StateMachine.Enter<AIControlledState>();
        }

        private void SetHUDState()
        {
            if (ControlledPlayerIsInAttack)
                _gameplayHud.StateMachine.Enter<AttackState, PlayerFacade>(_controlledPlayer);
            else
                _gameplayHud.StateMachine.Enter<DefenceState>();
        }

        private void SetCurrentPlayer(PlayerFacade player) =>
            _controlledPlayer = player;

        private void SubscribeOnChangePlayerInput() =>
            InputService.ChangePlayerButtonPressed += SwapControlledPlayer;

        private void UnsubscribeOnChangePlayerInput() =>
            InputService.ChangePlayerButtonPressed -= SwapControlledPlayer;

        private void SwapControlledPlayer()
        {
            if (ControlledPlayerIsInAttack)
                return;

            SetCurrentPlayer(NotControlledPlayer);
            SetPlayersState();
            SetHUDState();
        }
    }
}