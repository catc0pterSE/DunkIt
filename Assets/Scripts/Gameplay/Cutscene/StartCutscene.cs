using System;
using System.Linq;
using Cinemachine;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.NPC.Referee.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Minigame;
using Modules.MonoBehaviour;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Gameplay.Cutscene
{
    using Ball.MonoBehavior;

    public class StartCutscene : SwitchableMonoBehaviour, ICutscene
    {
        [SerializeField] private PlayableDirector _director;
        [SerializeField] private CinemachineVirtualCamera _refereeCamera;
        [SerializeField] private CinemachineVirtualCamera _ballCamera;
        [SerializeField] private CinemachineTargetGroup _playerTeamTargetGroup;
        [SerializeField] private CinemachineTargetGroup _enemyTeamTargetGroup;
        [SerializeField] private JumpBall _jumpBallMiniGame;

        private PlayerFacade _jumpBallPlayer;
        private EnemyFacade _jumpBallEnemy;
        
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
            _jumpBallMiniGame.Wined += GiveBallToPlayer;
            _jumpBallMiniGame.Lost += GiveBallToEnemy;
            _director.stopped += Finish;
        }

        private void OnDisable()
        {
            _jumpBallMiniGame.Wined -= GiveBallToPlayer;
            _jumpBallMiniGame.Lost -= GiveBallToEnemy;
            _director.stopped -= Finish;
        }

        public StartCutscene Initialize(CinemachineBrain gameplayCamera, PlayerFacade[] playerTeam,
            EnemyFacade[] enemyTeam, Referee referee, Ball ball)
        {
            PlayerFacade player1 = playerTeam[0];
            _jumpBallPlayer = player1;
            PlayerFacade player2 = playerTeam[1];
            EnemyFacade enemy1 = enemyTeam[0];
            _jumpBallEnemy = enemy1;
            EnemyFacade enemy2 = enemyTeam[1];

            _refereeCamera.LookAt = referee.transform;
            _ballCamera.LookAt = ball.transform;
            _playerTeamTargetGroup.m_Targets[0].target = player1.transform;
            _playerTeamTargetGroup.m_Targets[1].target = player2.transform;
            _enemyTeamTargetGroup.m_Targets[0].target = enemy1.transform;
            _enemyTeamTargetGroup.m_Targets[1].target = enemy2.transform;
            _director.SetGenericBinding(CinemachineTrackAsset, gameplayCamera);
            _director.SetGenericBinding(Player1TrackAsset, player1.Animator);
            _director.SetGenericBinding(Player2TrackAsset, player2.Animator);
            _director.SetGenericBinding(Enemy1TrackAsset, enemy1.Animator);
            _director.SetGenericBinding(Enemy2TrackAsset, enemy2.Animator);
            _director.SetGenericBinding(RefereeTrackAsset, referee.Animator);

            referee.TakeBall();

            return this;
        }

        public void Run()
        {
            _director.Play();
        }

        private void GiveBallToPlayer()
        {
            _jumpBallPlayer.TakeBall();
            EndCutscene();
        }


        private void GiveBallToEnemy()
        {
            _jumpBallEnemy.TakeBall();
            EndCutscene();
        }
            

        private void EndCutscene()
        {
            _director.Stop();
        }

        private void Finish(PlayableDirector director)
        {
            Finished?.Invoke();
        }
    }
}