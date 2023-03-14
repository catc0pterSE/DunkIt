using System;
using Gameplay.Character.Player.MonoBehaviour.Distance;
using UnityEngine;

namespace Gameplay.Character.Player.MonoBehaviour.Brains
{
    public class InputControlledAttackBrain : InputControlledBrain, IAttackEventLauncher

    {
    [SerializeField] private TargetTracker _targetTracker;

    private void OnEnable()
    {
        InputService.ThrowButtonDown += InitiateThrow;
        InputService.PassButtonDown += InitiatePass;
        InputService.DunkButtonDown += InitiateDunk;
    }

    private void OnDisable()
    {
        InputService.ThrowButtonDown -= InitiateThrow;
        InputService.PassButtonDown -= InitiatePass;
        InputService.DunkButtonDown -= InitiateDunk;
    }

    public event Action<PlayerFacade> ThrowInitiated;
    public event Action<PlayerFacade> PassInitiated;
    public event Action<PlayerFacade> DunkInitiated;

    private void InitiateThrow()
    {
        if (_targetTracker.IsInThrowDistance && Host.OwnsBall)
            ThrowInitiated?.Invoke(Host);
    }

    private void InitiatePass()
    {
        if (_targetTracker.IsInPassDistance && Host.OwnsBall)
            PassInitiated?.Invoke(Host);
    }

    private void InitiateDunk()
    {
        if (_targetTracker.IsInDunkDistance && Host.OwnsBall)
            DunkInitiated?.Invoke(Host);
    }
    }
}