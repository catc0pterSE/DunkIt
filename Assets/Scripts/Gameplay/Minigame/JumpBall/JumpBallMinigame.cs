using System;
using System.Linq;
using Cinemachine;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Modules.MonoBehaviour;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Gameplay.Minigame.JumpBall
{
    using Ball.MonoBehavior;

    public class JumpBallMinigame : SwitchableMonoBehaviour, IMinigame
    {
        [SerializeField] private PlayableDirector _director;
        [SerializeField] private JumpBallUI _uiMinigame;
        [SerializeField] private CinemachineVirtualCamera _ballCamera;
        [SerializeField] private CinemachineVirtualCamera _refereeCamera;

        private TimelineAsset TimelineAsset => _director.playableAsset as TimelineAsset;

        private TrackAsset CinemachineTrackAsset => TimelineAsset.GetOutputTracks()
            .FirstOrDefault(track => track.name == "CinemachineTrack");

        private TrackAsset PlayerTrackAsset => TimelineAsset.GetOutputTracks()
            .FirstOrDefault(track => track.name == "PlayerAnimation");

        private TrackAsset EnemyTrackAsset => TimelineAsset.GetOutputTracks()
            .FirstOrDefault(track => track.name == "EnemyAnimation");

        private TrackAsset RefereeTrackAsset => TimelineAsset.GetOutputTracks()
            .FirstOrDefault(track => track.name == "RefereeAnimation");

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

        public JumpBallMinigame Initialize(CinemachineBrain gameplayCamera, Referee referee, PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam, Ball ball)
        {
            PlayerFacade player = playerTeam[0];
            EnemyFacade enemy = enemyTeam[0];

            _ballCamera.LookAt = ball.transform;
            _refereeCamera.LookAt = referee.transform;
            _director.SetGenericBinding(CinemachineTrackAsset, gameplayCamera);
            _director.SetGenericBinding(PlayerTrackAsset, player.Animator);
            _director.SetGenericBinding(EnemyTrackAsset, enemy.Animator);
            _director.SetGenericBinding(RefereeTrackAsset, referee.Animator);

            referee.TakeBall();

            return this;
        }

        private void OnWon()
        {
            Won?.Invoke();
            End();
        }

        private void OnLost()
        {
            Lost?.Invoke();
            End();
        }

        private void End() =>
            _director.Stop();

        public void Launch() =>
            _director.Play();
    }
}