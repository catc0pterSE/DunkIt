using System;
using System.Linq;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.TriggerZone
{
    public class FightForBallTriggerZone : SwitchableMonoBehaviour
    {
        [SerializeField] private PlayerFacade _host;

        private PlayerFacade[] _allies;

        public void Initialize(PlayerFacade[] allies) =>
            _allies = allies;

        public event Action<PlayerFacade[]> FightForBallStarted;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerFacade basketballPlayer) == false)
                return;

            if (basketballPlayer == _host)
                return;

            if (_allies.Contains(basketballPlayer))
                return;

            PlayerFacade[] participants = new PlayerFacade[]
            {
                _host,
                basketballPlayer
            };

            FightForBallStarted?.Invoke(participants);
        }
    }
}