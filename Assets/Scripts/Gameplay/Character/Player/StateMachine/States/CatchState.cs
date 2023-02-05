using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using UnityEngine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class CatchState : IParameterlessState
    {
        private readonly PlayerFacade _player;

        public CatchState(PlayerFacade player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.DisableAIControlledBrain();
            _player.DisableInputControlledBrain();
            _player.FocusOnAlly();
            _player.RotateToAlly();
            _player.EnableCatcher();
        }

        public void Exit()
        {
            _player.DisableCatcher();
        }
    }
}