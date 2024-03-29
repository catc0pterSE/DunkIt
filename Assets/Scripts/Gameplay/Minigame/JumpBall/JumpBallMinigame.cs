﻿using System;
using System.Linq;
using Cinemachine;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Infrastructure.Input.InputService;
using Modules.MonoBehaviour;
using UnityEngine;
using UnityEngine.Playables;
using Utility.Constants;
using Utility.Extensions;

namespace Gameplay.Minigame.JumpBall
{
    public class JumpBallMinigame : SwitchableMonoBehaviour, IMinigame
    {
        [SerializeField] private PlayableDirector _director;
        [SerializeField] private JumpBallUI _interface;
        [SerializeField] private CinemachineVirtualCamera _ballCamera;
        [SerializeField] private CinemachineVirtualCamera _refereeCamera;

        public void Initialize
        (
            CinemachineBrain gameplayCamera,
            Referee referee,
            PlayerFacade[] playerTeam,
            PlayerFacade[] enemyTeam,
            Ball.MonoBehavior.Ball ball,
            IInputService inputService
        )
        {
            _interface.Initialize(inputService);

            PlayerFacade[] leftTeam;
            PlayerFacade[] rightTeam;
            
            if (playerTeam.First().LeftSide) //TODO: costyl
            {
                leftTeam = playerTeam;
                rightTeam = enemyTeam;
            }
            else
            {
                leftTeam = enemyTeam;
                rightTeam = playerTeam;
            }
            
            PlayerFacade leftPlayer = leftTeam[NumericConstants.PrimaryTeamMemberIndex];
            PlayerFacade rightPlayer = rightTeam[NumericConstants.PrimaryTeamMemberIndex];

            _ballCamera.LookAt = ball.transform;
            _refereeCamera.LookAt = referee.transform;
            

            _director.BindCinemachineBrain(TimelineTrackNames.CinemachineTrackName, gameplayCamera);
            _director.BindAnimator(TimelineTrackNames.LeftTeamPrimaryPlayerAnimationTrackName, leftPlayer.Animator);
            _director.BindAnimator(TimelineTrackNames.RightTeamPrimaryPlayerAnimationTrackName, rightPlayer.Animator);
            _director.BindAnimator(TimelineTrackNames.RefereeAnimationTrackName, referee.Animator);

            ball.SetOwner(referee);
        }
        
        public event Action Won;

        public event Action Lost;

        private void OnEnable()
        {
            _interface.Won += OnWon;
            _interface.Lost += OnLost;
        }

        private void OnDisable()
        {
            _interface.Won -= OnWon;
            _interface.Lost -= OnLost;
        }

        private void OnWon()
        {
            End();
            Won?.Invoke();
        }

        private void OnLost()
        {
            End();
            Lost?.Invoke();
        }

        private void End() =>
            _director.Stop();

        public void Launch() =>
            _director.Play();
    }
}