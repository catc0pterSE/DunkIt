using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.TriggerZone
{
    public class FightForBallTriggerZone : SwitchableMonoBehaviour
    {
        [SerializeField] private PlayerFacade _host;

        private PlayerFacade _ally;

        public void Initialize(PlayerFacade ally) =>
            _ally = ally;

        public event Action<PlayerFacade[]> FightForBallStarted;
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerFacade basketballPlayer) == false)
                return;

            if ( basketballPlayer == _host || basketballPlayer == _ally)
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