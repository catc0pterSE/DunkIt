using System;
using Modules.MonoBehaviour;
using UnityEngine.Playables;

namespace Gameplay.Cutscene
{
    public interface ICutscene : ISwitchable
    {
        public void Run();
        public event Action Finished;
    }
}