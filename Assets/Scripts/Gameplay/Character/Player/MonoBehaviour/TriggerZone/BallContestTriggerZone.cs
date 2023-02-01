using System;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.TriggerZone
{
    public class BallContestTriggerZone : SwitchableMonoBehaviour
    {
        [SerializeField] private BasketballPlayer _host;

        public event Action<BasketballPlayer> Entered;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
            if (other.gameObject.TryGetComponent<BasketballPlayer>(out BasketballPlayer basketballPlayer) == false)
                return;

            if (_host.GetType() != basketballPlayer.GetType())
                Entered?.Invoke(basketballPlayer);
        }
    }
}