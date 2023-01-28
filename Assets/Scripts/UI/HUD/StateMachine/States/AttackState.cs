using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using UnityEngine;

namespace UI.HUD.StateMachine.States
{
    public class AttackState : IParameterState<PlayerFacade>
    {
        private readonly IGameplayHUD _gameplayHUD;
        private PlayerFacade _player;

        public AttackState(IGameplayHUD gameplayHUD)
        {
            _gameplayHUD = gameplayHUD;
        }
        
        public void Enter(PlayerFacade player)
        {
            _player = player;
            SubscribeHudOnCurrentPlayer();
        }

        public void Exit()
        {
            UnsubscribeHudOnCurrentPlayer();
        }
        
        private void SubscribeHudOnCurrentPlayer()
        {
            Debug.Log(_player.gameObject.name);
            _player.ThrowReached += _gameplayHUD.SetThrowAvailability;
            _player.DunkReached += _gameplayHUD.SetDunkAvailability;
        }

        private void UnsubscribeHudOnCurrentPlayer()
        {
            _player.ThrowReached -= _gameplayHUD.SetThrowAvailability;
            _player.DunkReached -= _gameplayHUD.SetDunkAvailability;
        }
    }
}