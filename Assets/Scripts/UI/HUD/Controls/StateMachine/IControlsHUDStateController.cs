using System;
using Infrastructure.ServiceManagement;
using Modules.LiveData;

namespace UI.HUD.Controls.StateMachine
{
    public interface IControlsHUDStateController: IService
    {
        public LiveData<Action<GameplayHUDStateMachine>> HudStateSelection { get; }
    }
}