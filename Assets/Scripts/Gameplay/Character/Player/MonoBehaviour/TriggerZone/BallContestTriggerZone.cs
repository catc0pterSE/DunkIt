using System;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour.TriggerZone
{
    public class BallContestTriggerZone : SwitchableMonoBehaviour
    {
        [SerializeField] private BasketballPlayerFacade _host;

        public event Action<PlayerFacade, EnemyFacade> BallContestStarted;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<BasketballPlayerFacade>(out BasketballPlayerFacade basketballPlayer) ==
                false)
                return;

            if (_host.GetType() == basketballPlayer.GetType())
                return;

            BasketballPlayerFacade[] participants = new BasketballPlayerFacade[]
            {
                _host,
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