using System;
using System.Collections.Generic;
using Gameplay.Camera.MonoBehaviour;
using Gameplay.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.NPC.Referee.MonoBehaviour;
using Gameplay.Player.MonoBehaviour;
using Gameplay.StateMachine.States.CutsceneStates;
using Infrastructure.StateMachine;
using Scene;

namespace Gameplay.StateMachine
{
    using Ball.MonoBehavior;
    using Modules.StateMachine;

    public class GameplayLoopStateMachine : StateMachine
    {
        public GameplayLoopStateMachine(
            PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam,
            Referee referee,
            Ball ball,
            CameraFacade camera,
            SceneConfig sceneConfig,
            GameStateMachine gameStateMachine
        )
        {
            States = new Dictionary<Type, IState>
            {
                [typeof(StartCutsceneState)] = new StartCutsceneState(playerTeam, enemyTeam, referee, ball, camera, sceneConfig),
            };
        }

        public void Run() =>
            Enter<StartCutsceneState>();
    }
}