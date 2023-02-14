using System;
using Infrastructure.Input;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;
using UnityEngine.Playables;

namespace Gameplay.Cutscene
{
    public class CutsceneSkipper : SwitchableMonoBehaviour
    {
        [SerializeField] private PlayableDirector _director;
        private IInputService _inputService;

        private IInputService InputService => _inputService ?? Services.Container.Single<IInputService>();

        private void OnEnable()
        {
            InputService.PointerDown += SkipCutscene;
        }

        private void OnDisable()
        {
            InputService.PointerDown -= SkipCutscene;
        }

        private void SkipCutscene()
        {
            _director.Stop();
        }
    }
}