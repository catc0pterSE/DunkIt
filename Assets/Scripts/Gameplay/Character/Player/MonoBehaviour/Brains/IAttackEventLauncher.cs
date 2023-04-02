using System;

namespace Gameplay.Character.Player.MonoBehaviour.Brains
{
    public interface IAttackEventLauncher
    {
        public event Action<PlayerFacade> ThrowInitiated;
        public event Action<PlayerFacade> PassInitiated;
        public event Action<PlayerFacade> DunkInitiated;
    }
}