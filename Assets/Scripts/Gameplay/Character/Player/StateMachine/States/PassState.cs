using Gameplay.Character.Player.MonoBehaviour;
using Modules.StateMachine;

namespace Gameplay.Character.Player.StateMachine.States
{
    
    public class PassState: IParameterlessState
    {
        private readonly MonoBehaviour.PlayerFacade _player;

        public PassState(MonoBehaviour.PlayerFacade player)
        {
            _player = player;
        }
        
        public void Enter()
        {
            _player.DisableAIControlledBrain();
            _player.DisableInputControlledBrain();
            _player.FocusOnAlly();
            _player.RotateToAlly(Pass);
        }

        private void Pass()
        {
            _player.EnablePasser();
            _player.Pass();
            _player.PassedBall += OnPassedBall;
        }

        private void OnPassedBall()
        {
            _player.PassedBall -= OnPassedBall;
            _player.FocusOnBall();
        }

        public void Exit()
        {
            _player.DisablePasser();
        }
    }
}