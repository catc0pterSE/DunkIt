﻿using System;
using System.Linq;
using Cinemachine;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Modules.MonoBehaviour;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Utility.Constants;

namespace Gameplay.Cutscene
{
    public class StartCutscene : SwitchableMonoBehaviour, ICutscene
    {
        [SerializeField] private PlayableDirector _director;
        [SerializeField] private CinemachineVirtualCamera _refereeCamera;
        [SerializeField] private CinemachineVirtualCamera _ballCamera;
        [SerializeField] private CinemachineTargetGroup _playerTeamTargetGroup;
        [SerializeField] private CinemachineTargetGroup _enemyTeamTargetGroup;
        
        public event Action Finished;

        private TimelineAsset TimelineAsset => _director.playableAsset as TimelineAsset;

        private TrackAsset CinemachineTrackAsset => TimelineAsset.GetOutputTracks()
            .FirstOrDefault(track => track.name == "CinemachineTrack");

        private TrackAsset Player1TrackAsset => TimelineAsset.GetOutputTracks()
            .FirstOrDefault(track => track.name == "Player1Animation");

        private TrackAsset Player2TrackAsset => TimelineAsset.GetOutputTracks()
            .FirstOrDefault(track => track.name == "Player2Animation");

        private TrackAsset Enemy1TrackAsset => TimelineAsset.GetOutputTracks()
            .FirstOrDefault(track => track.name == "Enemy1Animation");

        private TrackAsset Enemy2TrackAsset => TimelineAsset.GetOutputTracks()
            .FirstOrDefault(track => track.name == "Enemy2Animation");

        private TrackAsset RefereeTrackAsset => TimelineAsset.GetOutputTracks()
            .FirstOrDefault(track => track.name == "RefereeAnimation");

        private void OnEnable()
        {
            _director.stopped += Finish;
        }

        private void OnDisable()
        {
            _director.stopped -= Finish;
        }

        public StartCutscene Initialize(CinemachineBrain gameplayCamera, PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam, Referee referee)
        {
            PlayerFacade player1 = playerTeam[NumericConstants.PrimaryPlayerIndex];
            PlayerFacade player2 = playerTeam[NumericConstants.SecondaryPlayerIndex];
            EnemyFacade enemy1 = enemyTeam[NumericConstants.PrimaryPlayerIndex];
            EnemyFacade enemy2 = enemyTeam[NumericConstants.SecondaryPlayerIndex];

            _refereeCamera.LookAt = referee.transform;
            _playerTeamTargetGroup.m_Targets[NumericConstants.PrimaryPlayerIndex].target = player1.transform;
            _playerTeamTargetGroup.m_Targets[NumericConstants.SecondaryPlayerIndex].target = player2.transform;
            _enemyTeamTargetGroup.m_Targets[NumericConstants.PrimaryPlayerIndex].target = enemy1.transform;
            _enemyTeamTargetGroup.m_Targets[NumericConstants.SecondaryPlayerIndex].target = enemy2.transform;
            _director.SetGenericBinding(CinemachineTrackAsset, gameplayCamera);
            _director.SetGenericBinding(Player1TrackAsset, player1.Animator);
            _director.SetGenericBinding(Player2TrackAsset, player2.Animator);
            _director.SetGenericBinding(Enemy1TrackAsset, enemy1.Animator);
            _director.SetGenericBinding(Enemy2TrackAsset, enemy2.Animator);
            _director.SetGenericBinding(RefereeTrackAsset, referee.Animator);
            
            return this;
        }

        public void Run()
        {
            _director.Play();
        }

        private void Finish(PlayableDirector director)
        {
            Finished?.Invoke();
        }
    }
}