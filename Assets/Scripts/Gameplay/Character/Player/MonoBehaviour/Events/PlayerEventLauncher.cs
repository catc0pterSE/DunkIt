using System;
using Gameplay.Character.Player.MonoBehaviour.TargetTracking;
using Infrastructure.Mediator;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.Events
{
    public class PlayerEventLauncher : UnityEngine.MonoBehaviour
    {
        [SerializeField] private TargetTracker _targetTracker;
        [SerializeField] private PlayerFacade _host;
        private TeamsMediator _teamsMediator;

        public void Initialize(TeamsMediator teamsMediator)
        {
            _teamsMediator = teamsMediator;
        }

        public event Action<PlayerFacade> ThrowInitiated;
        public event Action<PlayerFacade, PlayerFacade> PassInitiated;
        public event Action<PlayerFacade> DunkInitiated;
        public event Action<PlayerFacade> ChangePlayerInitiated;

        public void InitiateChangePlayer()
        {
            if (_teamsMediator.TryGetNextToControl(_host,out PlayerFacade next))
                ChangePlayerInitiated?.Invoke(next);
        }

        public void InitiateThrow()
        {
            if (_targetTracker.IsInThrowDistance && _host.OwnsBall)
                ThrowInitiated?.Invoke(_host);
        }

        public void InitiatePass()
        {
            if (_targetTracker.CanPass && _host.OwnsBall)
            {
                PassInitiated?.Invoke(_host, _targetTracker.PassTarget);
            }
        }

        public void InitiateDunk()
        {
            if (_targetTracker.IsInDunkDistance && _host.OwnsBall)
                DunkInitiated?.Invoke(_host);
        }
    }
}