using System;
using Gameplay.Character.Player.MonoBehaviour.TargetTracking;
using Infrastructure.Input.InputService;
using Infrastructure.Mediator;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.Events
{
    public class PlayerEventLauncher : UnityEngine.MonoBehaviour
    {
        [SerializeField] private TargetTracker _targetTracker;
        [SerializeField] private PlayerFacade _host;

        private IInputService _inputService;
        private TeamsMediator _teamsMediator;

        public void Initialize(IInputService inputService, TeamsMediator teamsMediator)
        {
            _teamsMediator = teamsMediator;
            _inputService = inputService;
        }

        public event Action<PlayerFacade> ThrowInitiated;
        public event Action<PlayerFacade, PlayerFacade> PassInitiated;
        public event Action<PlayerFacade> DunkInitiated;
        public event Action<PlayerFacade> ChangePlayerInitiated;

        public void SubscribeOnThrowInput() =>
            _inputService.ThrowButtonDown += InitiateThrow;

        public void SubscribeOnPassInput() =>
            _inputService.PassButtonDown += InitiatePass;

        public void SubscribeOnDunkInput() =>
            _inputService.DunkButtonDown += InitiateDunk;

        public void SubscribeOnChangePlayerInput() =>
            _inputService.ChangePlayerButtonDown += InitiateChangePlayer;
        
        public void UnsubscribeFromThrowInput() =>
            _inputService.ThrowButtonDown -= InitiateThrow;

        public void UnsubscribeFromPassInput() =>
            _inputService.PassButtonDown -= InitiatePass;

        public void UnsubscribeFromDunkInput() =>
            _inputService.DunkButtonDown -= InitiateDunk;

        public void UnsubscribeFromChangePlayerInput() =>
            _inputService.ChangePlayerButtonDown -= InitiateChangePlayer;

        private void InitiateChangePlayer()
        {
            if (_teamsMediator.TryGetNextToControl(_host,out PlayerFacade next))
                ChangePlayerInitiated?.Invoke(next);
        }

        private void InitiateThrow()
        {
            if (_targetTracker.IsInThrowDistance && _host.OwnsBall)
                ThrowInitiated?.Invoke(_host);
        }

        private void InitiatePass()
        {
            if (_targetTracker.TrySelectAllyToPass(out PlayerFacade passTarget) && _host.OwnsBall)
            {
                PassInitiated?.Invoke(_host, passTarget);
            }
        }

        private void InitiateDunk()
        {
            if (_targetTracker.IsInDunkDistance && _host.OwnsBall)
                DunkInitiated?.Invoke(_host);
        }
    }
}