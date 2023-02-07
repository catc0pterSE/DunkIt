using System;
using Cinemachine;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
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
        [SerializeField] private JumpBallUI _uiMinigame;
        [SerializeField] private CinemachineVirtualCamera _ballCamera;
        [SerializeField] private CinemachineVirtualCamera _refereeCamera;

        public event Action Won;

        public event Action Lost;

        private void OnEnable()
        {
            _uiMinigame.Won += OnWon;
            _uiMinigame.Lost += OnLost;
        }

        private void OnDisable()
        {
            _uiMinigame.Won -= OnWon;
            _uiMinigame.Lost -= OnLost;
        }

        public JumpBallMinigame Initialize
        (
            CinemachineBrain gameplayCamera,
            Referee referee,
            PlayerFacade[] playerTeam,
            PlayerFacade[] enemyTeam,
            Ball.MonoBehavior.Ball ball
        )
        {
            PlayerFacade player = playerTeam[NumericConstants.PrimaryTeamMemberIndex];
            PlayerFacade enemy = enemyTeam[NumericConstants.PrimaryTeamMemberIndex];

            _ballCamera.LookAt = ball.transform;
            _refereeCamera.LookAt = referee.transform;

            _director.BindCinemachineBrain(TimelineTrackNames.CinemachineTrackName, gameplayCamera);
            _director.BindAnimator(TimelineTrackNames.PrimaryPlayerAnimationTrackName, player.Animator);
            _director.BindAnimator(TimelineTrackNames.PrimaryEnemyAnimationTrackName, enemy.Animator);
            _director.BindAnimator(TimelineTrackNames.RefereeAnimationTrackName, referee.Animator);

            ball.SetOwner(referee);

            return this;
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