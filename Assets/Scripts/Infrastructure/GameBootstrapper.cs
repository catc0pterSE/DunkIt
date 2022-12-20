using System;
using Infrastructure.StateMachine;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private Game _game;

        private void Awake()
        {
            _game = new Game();
            _game.StateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
    }
}
