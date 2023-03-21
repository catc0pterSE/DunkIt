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
            PlayerFacade[] playerTeam,
            PlayerFacade[] enemyTeam,
            Referee referee,
            IInputService inputService
        )
        {
            _cutsceneSkipper.Initialize(inputService);

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


            PlayerFacade leftPlayer1 = leftTeam[NumericConstants.PrimaryTeamMemberIndex];
            PlayerFacade leftPlayer2 = leftTeam[NumericConstants.SecondaryTeamMemberIndex];
            PlayerFacade rightPlayer1 = rightTeam[NumericConstants.PrimaryTeamMemberIndex];
            PlayerFacade rightPlayer2 = rightTeam[NumericConstants.SecondaryTeamMemberIndex];

            _refereeCamera.LookAt = referee.transform;
            _leftTeamTargetGroup.m_Targets[NumericConstants.PrimaryTeamMemberIndex].target = leftPlayer1.transform;
            _leftTeamTargetGroup.m_Targets[NumericConstants.SecondaryTeamMemberIndex].target = leftPlayer2.transform;
            _rightTeamTargetGroup.m_Targets[NumericConstants.PrimaryTeamMemberIndex].target = rightPlayer1.transform;
            _rightTeamTargetGroup.m_Targets[NumericConstants.SecondaryTeamMemberIndex].target = rightPlayer2.transform;

            _director.BindCinemachineBrain(TimelineTrackNames.CinemachineTrackName, gameplayCamera);
            _director.BindAnimator(TimelineTrackNames.LeftTeamPrimaryPlayerAnimationTrackName, leftPlayer1.Animator);
            _director.BindAnimator(TimelineTrackNames.LeftTeamSecondaryPlayerAnimationTrackName, leftPlayer2.Animator);
            _director.BindAnimator(TimelineTrackNames.RightTeamPrimaryPlayerAnimationTrackName, rightPlayer1.Animator);
            _director.BindAnimator(TimelineTrackNames.RightTeamSecondaryEnemyAnimationTrackName, rightPlayer2.Animator);
            _director.BindAnimator(TimelineTrackNames.RefereeAnimationTrackName, referee.Animator);

            return this;
        }

        public void Run() =>
            _director.Play();

        private void Finish(PlayableDirector director) =>
            Finished?.Invoke();
    }
}