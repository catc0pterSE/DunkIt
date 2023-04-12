using System.Linq;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.Transitions;
using Modules.StateMachine;
using Scene;
using UI;
using UI.HUD;
using Utility.Constants;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.Gameplay
{
    using Ball.MonoBehavior;

    public class DropBallState : StateWithTransitions, IParameterState<PlayerFacade>
    {
        private readonly PlayerFacade[] _leftTeam;
        private readonly PlayerFacade[] _rightTeam;
        private readonly Ball _ball;
        private readonly SceneInitials _sceneInitials;
        private readonly LoadingCurtain _loadingCurtain;

        private PlayerFacade _droppingPlayer;
        private IGameplayHUD _gameplayHUD;

        public DropBallState(PlayerFacade[] leftTeam, PlayerFacade[] rightTeam, Ball ball, SceneInitials sceneInitials, IGameplayHUD gameplayHUD,
            LoadingCurtain loadingCurtain, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _gameplayHUD = gameplayHUD;
            _leftTeam = leftTeam;
            _rightTeam = rightTeam;
            _ball = ball;
            _sceneInitials = sceneInitials;
            _loadingCurtain = loadingCurtain;

            Transitions = new ITransition[]
            {
                new AnyToPassTransition(leftTeam.Union(rightTeam).ToArray(), gameplayLoopStateMachine)
            };
        }

        public void Enter(PlayerFacade droppingPlayer)
        {
            base.Enter();
            _droppingPlayer = droppingPlayer;
            _loadingCurtain.FadeInFadeOut(() =>
            {
                ArrangePlayers();
                _ball.SetOwner(_droppingPlayer);
                SetPlayersStates();
            });
            
            _gameplayHUD.Enable();
        }

        public override void Exit()
        {
            _gameplayHUD.Disable();
        }

        private void ArrangePlayers()
        {
            for (int i = 0; i < NumericConstants.PlayersInTeam; i++)
            {
                _leftTeam[i].transform.CopyValuesFrom(_sceneInitials.LeftTeamDropBallPositions[i], false);
                _rightTeam[i].transform.CopyValuesFrom(_sceneInitials.RightTeamDropBallPositions[i], false);
            }
        }

        private void SetPlayersStates()
        {
            _leftTeam.Union(_rightTeam).Map(player =>
            {
                if (player == _droppingPlayer)
                    player.EnterDropBallState();
                else
                    player.EnterIdleState(_ball.Owner.transform.position);
            });
        }
    }
}