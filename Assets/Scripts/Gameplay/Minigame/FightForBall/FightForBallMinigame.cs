using System;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Minigame.FightForBall.UI;
using Modules.MonoBehaviour;
using UnityEngine;

namespace Gameplay.Minigame.FightForBall
{
    public class FightForBallMinigame : SwitchableMonoBehaviour, IMinigame
    {
        [SerializeField] private FightForBallUI _interface;

        private PlayerFacade _player;
        private PlayerFacade _enemy;

        public event Action Won
        {
            add => _interface.Won += value;
            remove => _interface.Won -= value;
        }

        public event Action Lost
        {
            add => _interface.Lost += value;
            remove => _interface.Lost -= value;
        }

        public void Initialize()
        {
        }

        public void Launch()
        {
            _interface.Enable();
        }
    }
}