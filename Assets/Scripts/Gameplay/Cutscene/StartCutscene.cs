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

namespace Gameplay.Cutscene
{
    public class StartCutscene : SwitchableMonoBehaviour, ICutscene
    {
        [SerializeField] private PlayableDirector _director;
        [SerializeField] private CutsceneSkipper _cutsceneSkipper;
        [SerializeField] private CinemachineVirtualCamera _refereeCamera;
        [SerializeField] private CinemachineTargetGroup _playerTeamTargetGroup;
        [SerializeField] private CinemachineTargetGroup _enemyTeamTargetGroup;

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

            PlayerFacade player1 = playerTeam[NumericConstants.PrimaryTeamMemberIndex];
            PlayerFacade player2 = playerTeam[NumericConstants.SecondaryTeamMemberIndex];
            PlayerFacade enemy1 = enemyTeam[NumericConstants.PrimaryTeamMemberIndex];
            PlayerFacade enemy2 = enemyTeam[NumericConstants.SecondaryTeamMemberIndex];

            _refereeCamera.LookAt = referee.transform;
            _playerTeamTargetGroup.m_Targets[NumericConstants.PrimaryTeamMemberIndex].target = player1.transform;
            _playerTeamTargetGroup.m_Targets[NumericConstants.SecondaryTeamMemberIndex].target = player2.transform;
            _enemyTeamTargetGroup.m_Targets[NumericConstants.PrimaryTeamMemberIndex].target = enemy1.transform;
            _enemyTeamTargetGroup.m_Targets[NumericConstants.SecondaryTeamMemberIndex].target = enemy2.transform;

            _director.BindCinemachineBrain(TimelineTrackNames.CinemachineTrackName, gameplayCamera);
            _director.BindAnimator(TimelineTrackNames.PrimaryPlayerAnimationTrackName, player1.Animator);
            _director.BindAnimator(TimelineTrackNames.SecondaryPlayerAnimationTrackName, player2.Animator);
            _director.BindAnimator(TimelineTrackNames.PrimaryEnemyAnimationTrackName, enemy1.Animator);
            _director.BindAnimator(TimelineTrackNames.SecondaryEnemyAnimationTrackName, enemy2.Animator);
            _director.BindAnimator(TimelineTrackNames.RefereeAnimationTrackName, referee.Animator);

            return this;
        }

        public void Run() =>
            _director.Play();

        private void Finish(PlayableDirector director) =>
            Finished?.Invoke();
    }
}