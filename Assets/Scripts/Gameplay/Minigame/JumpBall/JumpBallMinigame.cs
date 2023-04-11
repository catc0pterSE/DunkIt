using System;
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
            PlayerFacade[] leftTeam,
            PlayerFacade[] rightTeam,
            CinemachineBrain gameplayCamera,
            Referee referee,
            Ball.MonoBehavior.Ball ball,
            IInputService inputService
        )
        {
            _interface.Initialize(inputService);

            _ballCamera.LookAt = ball.transform;
            _refereeCamera.LookAt = referee.transform;

            _director.BindCinemachineBrain(TimelineTrackNames.CinemachineTrackName, gameplayCamera);

            for (int i = 0; i < NumericConstants.PlayersInTeam; i++)
            {
                _director.BindAnimator(TimelineTrackNames.GetLeftTeamAnimationTrackName(i), leftTeam[i].Animator);
                _director.BindAnimator(TimelineTrackNames.GetRightTeamAnimationTrackName(i), rightTeam[i].Animator);
            }
            
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