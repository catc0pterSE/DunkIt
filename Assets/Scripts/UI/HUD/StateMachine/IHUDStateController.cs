using System;
using Gameplay.Character.Player.MonoBehaviour;
using Infrastructure.ServiceManagement;
using Modules.LiveData;

namespace UI.HUD.StateMachine
{
    using Modules.StateMachine;
    
    public interface IHUDStateController: IService
    {
        public LiveData<Action<GameplayHUDStateMachine>> HudStateSelection { get; }
    }
}