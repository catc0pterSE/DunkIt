using System;
using UnityEngine.Playables;

namespace Gameplay.Cutscene
{
    public interface ICutscene
    {
        public void Run();
        public event Action Finished;
        public void Enable();
        public void Disable();
    }
}