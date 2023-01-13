using System;

namespace Gameplay.Cutscene
{
    public interface ICutscene
    {
        public event Action Finished;
        public void Run();
    }
}