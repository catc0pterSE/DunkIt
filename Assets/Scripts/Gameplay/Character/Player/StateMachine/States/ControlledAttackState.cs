using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;
using UnityEngine;

namespace Gameplay.Character.Player.StateMachine.States
{
    public class ControlledAttackState : IParameterState<Transform>
    {
        private readonly PlayerFacade _playerFacade;

        public ControlledAttackState(PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
        }

        public void Enter(Transform lookTarget)
        {
            _playerFacade.EnableInputControlledBrain();
            _playerFacade.EnablePlayerMover();
            _playerFacade.PrioritizeCamera();
            _playerFacade.FocusOnEnemyBasket();
        }


        public void Exit()
        {
            _playerFacade.DisablePlayerMover();
            _playerFacade.DisableInputControlledBrain();
            _playerFacade.DeprioritizeCamera();
        }
            
        
    }
}