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

        public void Initialize(IInputService inputService)
        {
            _inputService = inputService;
        }
            

        private void OnEnable()
        {
            _inputService.PointerDown += SkipCutscene; 
        }
            

        private void OnDisable() 
        {
            
            _inputService.PointerDown -= SkipCutscene;
        }
            

        private void SkipCutscene() =>
            _director.Stop();
    }
}