using System;
using System.Collections.Generic;
using Cinemachine;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.CutsceneStates;
using Gameplay.StateMachine.States.Gameplay;
using Gameplay.StateMachine.States.MinigameStates;
using Infrastructure.CoroutineRunner;
using Infrastructure.StateMachine;
using Scene;
using UI.HUD;
using UI.HUD.Mobile;

namespace Gameplay.StateMachine
{
    using Ball.MonoBehavior;
    using Modules.StateMachine;

    public class GameplayLoopStateMachine : StateMachine
    {
        public GameplayLoopStateMachine(PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam,
            Referee referee,
            CinemachineBrain camera,
            IGameplayHUD gameplayHUD,
            Ball ball,
            SceneConfig sceneConfig,
            ICoroutineRunner coroutineRunner,
            GameStateMachine gameStateMachine)
        {
            States = new Dictionary<Type, IState>
            {
                [typeof(StartCutsceneState)] = new StartCutsceneState(playerTeam, enemyTeam, referee, camera, ball, gameplayHUD, this),
                [typeof(JumpBallState)] = new JumpBallState(playerTeam, enemyTeam, referee, ball, camera, gameplayHUD, this),
                [typeof(GameplayState)] = new GameplayState(playerTeam, ball, gameplayHUD, sceneConfig)
            };
        }

        public void Run() =>
            Enter<StartCutsceneState>();
    }
}