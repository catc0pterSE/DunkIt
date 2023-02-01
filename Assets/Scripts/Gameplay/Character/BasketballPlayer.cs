using System;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.TriggerZone;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Character
{
    public abstract class BasketballPlayer : Character
    {
        [SerializeField] private BallContestTriggerZone _ballContestTriggerZone;

        public event Action<PlayerFacade, EnemyFacade> BallContestStarted;

        public abstract event Action BallThrown;
        
        private void OnEnable()
        {
            _ballContestTriggerZone.Entered += OnBallContestTriggerZoneEntered;
        }

        private void OnDisable()
        {
            _ballContestTriggerZone.Entered -= OnBallContestTriggerZoneEntered;
        }

        public void EnableBallContestTrigger() =>
            _ballContestTriggerZone.Enable();

        public void DisableBallContestTrigger() =>
            _ballContestTriggerZone.Disable();

        private void OnBallContestTriggerZoneEntered(BasketballPlayer basketballPlayer)
        {
            BasketballPlayer[] participants = new BasketballPlayer[]
            {
                this,
                basketballPlayer
            };

            PlayerFacade player =
                participants.FindFirstOrNull(participant => participant is PlayerFacade) as PlayerFacade;

            EnemyFacade enemy =
                participants.FindFirstOrNull(participant => participant is EnemyFacade) as EnemyFacade;
            
            if (player != null & enemy != null)
                BallContestStarted?.Invoke(player, enemy);
            else
                throw new NullReferenceException("there is no Player or Enemy in BallContest participants");
        }
    }
}