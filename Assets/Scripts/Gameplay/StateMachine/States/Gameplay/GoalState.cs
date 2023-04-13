using System;
using System.Collections;
using System.Linq;
using Cinemachine;
using Gameplay.Character.Player.MonoBehaviour;
using Infrastructure.CoroutineRunner;
using Modules.StateMachine;
using Scene;
using Scene.Ring;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.Gameplay
{
    public class GoalState : IParameterState<Ring>
    {
        private const float DelaySeconds = 2;

        private readonly PlayerFacade[] _leftTeam;
        private readonly PlayerFacade[] _rightTeam;
        private readonly SceneInitials _sceneInitials;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly GameplayLoopStateMachine _gameplayLoopStateMachine;

        private Coroutine _delayedEnterNextStateRoutine;

        public GoalState(PlayerFacade[] leftTeam, PlayerFacade[] rightTeam, SceneInitials sceneInitials,
            ICoroutineRunner coroutineRunner, GameplayLoopStateMachine gameplayLoopStateMachine)
        {
            _leftTeam = leftTeam;
            _rightTeam = rightTeam;
            _sceneInitials = sceneInitials;
            _coroutineRunner = coroutineRunner;
            _gameplayLoopStateMachine = gameplayLoopStateMachine;
            //TODO: endGame transition
        }

        public void Enter(Ring ring)
        {
            _leftTeam.Union(_rightTeam).Map(player => player.EnterIdleState(ring.transform.position));
            CinemachineVirtualCamera camera = ring.GoalCamera;
            camera.Prioritize();
            camera.LookAt = ring.BallDunkPoint;
                //TODO: goal particles, score++

            PlayerFacade droppingPlayer;
            if (ring == _sceneInitials.LeftRing)
                droppingPlayer = _leftTeam.First();
            else if (ring == _sceneInitials.RightRing)
                droppingPlayer = _rightTeam.First();
            else
                throw new ArgumentException("unknown ring");
            
            if (_delayedEnterNextStateRoutine !=null)                        //TODO: to transition, on end of particles
                _coroutineRunner.StopCoroutine(_delayedEnterNextStateRoutine);
            
            _delayedEnterNextStateRoutine = _coroutineRunner.StartCoroutine(DelayedEnterNextState(droppingPlayer));
        }

        public void Exit()
        {
        }

        private IEnumerator DelayedEnterNextState(PlayerFacade droppingPlayer)
        {
            yield return new WaitForSeconds(DelaySeconds);
            _gameplayLoopStateMachine.Enter<DropBallState, PlayerFacade>(droppingPlayer);
        }
    }
}