using System;

namespace Gameplay.Minigame
{
    public interface IMinigame
    {
        public event Action Won;
        public event Action Lost;
        public void Launch();
        public void Enable();
        public void Disable();
    }
}