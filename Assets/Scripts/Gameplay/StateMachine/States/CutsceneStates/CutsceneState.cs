using System;
using Gameplay.Camera.MonoBehaviour;
using Gameplay.Camera.StateMachine.States;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Cutscene;
using Infrastructure.CoroutineRunner;
using Modules.StateMachine;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.CutsceneStates
{
    using Ball.MonoBehavior;

    public abstract class CutsceneState : IParameterlessState
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly EnemyFacade[] _enemyTeam;
        private readonly CameraFacade _camera;
        private readonly ICutscene _cutscene;

        protected CutsceneState
        (
            PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam,
            CameraFacade camera,
            ICutscene cutscene
        )
        {
            _playerTeam = playerTeam;
            _enemyTeam = enemyTeam;
            _camera = camera;
            _cutscene = cutscene;
        }


        public virtual void Enter()
        {
            SetCharactersStates();
            SetCameraState();
            SubscribeOnCutscene();
            LaunchCutscene();
        }

        public abstract void Exit();

        protected abstract void EnterNextState();

        private void SetCharactersStates()
        {
            _playerTeam.Map(player => player.StateMachine.Enter<Character.Player.StateMachine.States.CutsceneState>());
            _enemyTeam.Map(enemy => enemy.StateMachine.Enter<Character.NPC.EnemyPlayer.StateMachine.States.CutsceneState>());
        }
        
        private void SetCameraState() =>
            _camera.StateMachine.Enter<Camera.StateMachine.States.CutsceneState>();
        
        private void SubscribeOnCutscene() =>
            _cutscene.Finished += EnterNextState;
        
        private void LaunchCutscene()=>
            _cutscene.Run();
    }
}