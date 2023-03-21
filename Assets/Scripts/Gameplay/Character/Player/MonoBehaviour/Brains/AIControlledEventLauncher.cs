using System;
using Gameplay.Character.Player.MonoBehaviour.TargetTracking;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.Brains
{
    public class AIControlledEventLauncher : UnityEngine.MonoBehaviour, IAttackEventLauncher
    {
        [SerializeField] private TargetTracker _targetTracker;
        [SerializeField] private PlayerFacade _host;
        
        public event Action<PlayerFacade> ThrowInitiated;
        public event Action<PlayerFacade> PassInitiated;
        public event Action<PlayerFacade> DunkInitiated;
        
        public void InitiateThrow()
        {
            if (_targetTracker.IsInThrowDistance && _host.OwnsBall)
                ThrowInitiated?.Invoke(_host);
        }

        public void InitiatePass()
        {
            if (_targetTracker.IsInPassDistance && _host.OwnsBall)
                PassInitiated?.Invoke(_host);
        }

        public void InitiateDunk()
        {
            if (_targetTracker.IsInDunkDistance && _host.OwnsBall)
                DunkInitiated?.Invoke(_host);
        }
    }
}