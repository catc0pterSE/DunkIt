using System;
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

namespace Gameplay.Cutscene
{
    public class StartCutscene : SwitchableMonoBehaviour, ICutscene
    {
        [SerializeField] private PlayableDirector _director;
        [SerializeField] private CutsceneSkipper _cutsceneSkipper;
        [SerializeField] private CinemachineVirtualCamera _refereeCamera;
        [SerializeField] private CinemachineTargetGroup _leftTeamTargetGroup;
        [SerializeField] private CinemachineTargetGroup _rightTeamTargetGroup;

        public event Action Finished;

        private void OnEnable() =>
            _director.stopped += Finish;

        private void OnDisable() =>
            _director.stopped -= Finish;

        public StartCutscene Initialize
        (
            CinemachineBrain gameplayCamera,
            PlayerFacade[] leftTeam,
            PlayerFacade[] rightTeam,
            Referee referee,
            IInputService inputService
        )
        {
            _cutsceneSkipper.Initialize(inputService);

            _refereeCamera.LookAt = referee.transform;

            for (int i = 0; i < NumericConstants.PlayersInTeam; i++)
            {
                _leftTeamTargetGroup.m_Targets[i].target = leftTeam[i].transform;
                _rightTeamTargetGroup.m_Targets[i].target = rightTeam[i].transform;
            }

            _director.BindCinemachineBrain(TimelineTrackNames.CinemachineTrackName, gameplayCamera);

            for (int i = 0; i < NumericConstants.PlayersInTeam; i++)
            {
                _director.BindAnimator(TimelineTrackNames.GetRightTeamAnimationTrackName(i), rightTeam[i].Animator);
                _director.BindAnimator(TimelineTrackNames.GetLeftTeamAnimationTrackName(i), leftTeam[i].Animator);
            }
            
            _director.BindAnimator(TimelineTrackNames.RefereeAnimationTrackName, referee.Animator);

            return this;
        }

        public void Run() =>
            _director.Play();

        private void Finish(PlayableDirector director) =>
            Finished?.Invoke();
    }
}