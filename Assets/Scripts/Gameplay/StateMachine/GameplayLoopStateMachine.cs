using System;
using System.Collections.Generic;
using Gameplay.Camera.MonoBehaviour;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.StateMachine.States.CutsceneStates;
using Infrastructure.CoroutineRunner;
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
            ICoroutineRunner coroutineRunner,
            GameStateMachine gameStateMachine
        )
        {
            States = new Dictionary<Type, IState>
            {
                [typeof(StartCutsceneState)] = new StartCutsceneState(playerTeam, enemyTeam, referee, ball, camera, sceneConfig, coroutineRunner, this),
            };
        }

        public void Run() =>
            Enter<StartCutsceneState>();
    }
}