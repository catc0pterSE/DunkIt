using System;
using Modules.MonoBehaviour;

namespace Gameplay.Minigame
{
    public interface IMinigame : ISwitchable
    {
        public event Action Won;
        public event Action Lost;
        public void Launch();
    }
}