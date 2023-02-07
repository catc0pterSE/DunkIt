using System;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour.TriggerZone
{
    public class BallContestTriggerZone : SwitchableMonoBehaviour
    {
        [SerializeField] private PlayerFacade _host;

        public event Action<PlayerFacade, PlayerFacade> BallContestStarted;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerFacade basketballPlayer) ==
                false)
                return;

            if (_host.GetType() == basketballPlayer.GetType())
                return;

            PlayerFacade[] participants = new PlayerFacade[]
            {
                _host,
                basketballPlayer
            };

            PlayerFacade player =
                participants.FindFirstOrNull(participant => participant.IsPlayable ) ;

            PlayerFacade enemy =
                participants.FindFirstOrNull(participant => participant.IsPlayable == false) ;

            if (player != null && enemy != null)
                BallContestStarted?.Invoke(player, enemy);
            else
                throw new NullReferenceException("there is no playable or not playable characters in BallContest participants");
        }
    }
}