using System;
using Gameplay.Character.NPC.EnemyPlayer.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.MonoBehaviour.TriggerZone;
using UnityEngine;

namespace Gameplay.Character
{
    public abstract class BasketballPlayerFacade : CharacterFacade
    {
        [SerializeField] private BallContestTriggerZone _ballContestTriggerZone;
        
        
        public event Action<PlayerFacade, EnemyFacade> BallContestStarted
        {
            add => _ballContestTriggerZone.BallContestStarted += value;
            remove => _ballContestTriggerZone.BallContestStarted -= value;
        }

        public void EnableBallContestTriggerZone() =>
            _ballContestTriggerZone.Enable();

        public void DisableBallContestTriggerZone() =>
            _ballContestTriggerZone.Disable();
    }
}