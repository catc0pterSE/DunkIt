using System;

namespace Gameplay.Minigame
{
    public interface IMinigame
    {
        public event Action Wined;
        public event Action Lost;
        public void Launch();
    }
}