using Gameplay.Character.Player.MonoBehaviour;
using Infrastructure.PlayerService;
using Modules.MonoBehaviour;
using UI.HUD.StateMachine;
using UnityEngine;

namespace UI.HUD
{
    public interface IGameplayHUD : ISwitchable
    {
        public void SetThrowAvailability(bool isAvailable);

        public void SetDunkAvailability(bool isAvailable);

        public void SetPassAvailability(bool isAvailable);

        public void SetChangePlayerAvailability(bool isAvailable);

        public IGameplayHUD Initialize(PlayerFacade[] indicationTargets, Camera gameplayCamera, IHUDStateController hudStateController);
    }
}