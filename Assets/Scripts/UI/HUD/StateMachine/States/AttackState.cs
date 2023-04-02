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
            _gameplayHUD.SetChangePlayerAvailability(false);
            SubscribeHudOnCurrentPlayer();
        }

        public void Exit()
        {
            UnsubscribeHudFromCurrentPlayer();
            _gameplayHUD.SetDunkAvailability(false);
            _gameplayHUD.SetPassAvailability(false);
            _gameplayHUD.SetThrowAvailability(false);
        }
        
        private void SubscribeHudOnCurrentPlayer()
        {
            _player.ThrowReached += _gameplayHUD.SetThrowAvailability;
            _player.DunkReached += _gameplayHUD.SetDunkAvailability;
            _player.PassReached += _gameplayHUD.SetPassAvailability;
        }

        private void UnsubscribeHudFromCurrentPlayer()
        {
            _player.ThrowReached -= _gameplayHUD.SetThrowAvailability;
            _player.DunkReached -= _gameplayHUD.SetDunkAvailability;
            _player.PassReached -= _gameplayHUD.SetPassAvailability;
        }
    }
}