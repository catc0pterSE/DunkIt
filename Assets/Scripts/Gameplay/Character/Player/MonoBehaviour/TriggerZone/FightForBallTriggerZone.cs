using System;
using Modules.MonoBehaviour;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Character.Player.MonoBehaviour.TriggerZone
{
    public class FightForBallTriggerZone : SwitchableMonoBehaviour
    {
        [SerializeField] private PlayerFacade _host;
        [SerializeField] private PlayerFacade _ally;

        public event Action<PlayerFacade[]> FightForBallStarted;
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerFacade basketballPlayer) == false)
                return;

            if (_host == basketballPlayer)
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