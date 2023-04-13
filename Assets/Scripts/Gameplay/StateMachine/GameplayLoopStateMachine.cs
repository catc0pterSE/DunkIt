using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Camera;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.CutsceneStates;
using Gameplay.StateMachine.States.Gameplay;
using Gameplay.StateMachine.States.MinigameStates;
using Infrastructure.CoroutineRunner;
using Infrastructure.Factory;
using Infrastructure.Input.InputService;
using Infrastructure.StateMachine;
using Modules.StateMachine;
using Scene;
using UI;

namespace Gameplay.StateMachine
{
    public class GameplayLoopStateMachine : Modules.StateMachine.StateMachine
    {
        public GameplayLoopStateMachine
        (
            PlayerFacade[] leftTeam,
            PlayerFacade[] rightTeam,
            Referee referee,
            CameraFacade camera,
            Ball.MonoBehavior.Ball ball,
            SceneInitials sceneInitials,
            LoadingCurtain loadingCurtain,
            ICoroutineRunner coroutineRunner,
            IGameObjectFactory gameObjectFactory,
            IInputService inputService,
            GameStateMachine gameStateMachine
        )
        {
            PlayerFacade[] playableTeam;
            PlayerFacade[] notPlayableTeam;
            
            if (leftTeam.All(player => player.CanBeLocalControlled) && rightTeam.All((player => player.CanBeLocalControlled == false)))   //TODO: only for minigames temporary
            {
                playableTeam = leftTeam;
                notPlayableTeam = rightTeam;
            }
            else if (leftTeam.All(player => player.CanBeLocalControlled == false) && rightTeam.All(player => player.CanBeLocalControlled))
            {
                playableTeam = rightTeam;
                notPlayableTeam = leftTeam;
            }
            else
            {
                throw new ArgumentException("could not identify controlled and npc team");
            }

            States = new Dictionary<Type, IState>
            {
                [typeof(StartCutsceneState)] = new StartCutsceneState(leftTeam, rightTeam, referee, camera.CinemachineBrain, ball, this, gameObjectFactory, inputService),
                [typeof(JumpBallState)] = new JumpBallState(leftTeam, rightTeam, playableTeam, notPlayableTeam, referee, ball, camera.CinemachineBrain,  this, gameObjectFactory, inputService),
                [typeof(AttackDefenceState)] = new AttackDefenceState(leftTeam, rightTeam, ball, sceneInitials,  this, coroutineRunner),
                [typeof(BallChasingState)] = new BallChasingState(leftTeam, rightTeam, ball, sceneInitials, this),
                [typeof(PassState)] = new PassState(ball,  this),
                [typeof(DunkState)] = new DunkState( leftTeam, rightTeam, sceneInitials, ball,  this),
                [typeof(ThrowState)] = new ThrowState(ball, this),
                [typeof(DropBallState)] = new DropBallState(leftTeam, rightTeam, ball, sceneInitials, loadingCurtain, this),
                [typeof(FightForBallState)] = new FightForBallState(playableTeam, notPlayableTeam, ball, this, gameObjectFactory),
                [typeof(GoalState)] = new GoalState(leftTeam, rightTeam, sceneInitials, coroutineRunner, this)
            };
        }

        public void Run() =>
            Enter<StartCutsceneState>();
    }
}