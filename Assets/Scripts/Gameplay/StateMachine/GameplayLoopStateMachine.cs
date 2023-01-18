using System;
using System.Collections.Generic;
using Cinemachine;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.HUD;
using Gameplay.StateMachine.States.CutsceneStates;
using Gameplay.StateMachine.States.MiniGameStates;
using Infrastructure.CoroutineRunner;
using Infrastructure.StateMachine;
using Scene;

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
            GameplayHUD gameplayHUD,
            Ball ball,
            SceneConfig sceneConfig,
            ICoroutineRunner coroutineRunner,
            GameStateMachine gameStateMachine)
        {
            States = new Dictionary<Type, IState>
            {
                [typeof(StartCutsceneState)] = new StartCutsceneState(playerTeam, enemyTeam, referee, ball, camera, gameplayHUD, this),
                
            };
        }

        public void Run() =>
            Enter<StartCutsceneState>();
    }
}