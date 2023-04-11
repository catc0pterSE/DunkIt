using System;
using Gameplay.Character.Player.MonoBehaviour;
using Infrastructure.ServiceManagement;
using Modules.LiveData;

namespace Infrastructure.PlayerService
{
    using Modules.StateMachine;

    public interface IPlayerService : IService
    {
        public PlayerFacade CurrentControlled { get; }
        public void Set(PlayerFacade player);
    }
}