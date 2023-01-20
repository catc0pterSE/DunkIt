﻿using System.Linq;
using Gameplay.Character.Player.MonoBehaviour;
using Gameplay.Character.Player.StateMachine.States;
using Gameplay.HUD;
using Modules.StateMachine;
using Scene;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.StateMachine.States.Gameplay
{
    using Ball.MonoBehavior;
    using Character;

    public class GameplayState : StateWithTransitions
    {
        private readonly PlayerFacade[] _playerTeam;
        private readonly Ball _ball;
        private readonly GameplayHUD _hud;
        private readonly SceneConfig _sceneConfig;
        private PlayerFacade _currentControlledPlayer;

        public GameplayState(PlayerFacade[] playerTeam, Ball ball, GameplayHUD hud, SceneConfig sceneConfig) : base(new ITransition[]
            {
            }
        )
        {
            _playerTeam = playerTeam;
            _ball = ball;
            _hud = hud;
            _sceneConfig = sceneConfig;
        }

        public override void Enter()
        {
            base.Enter();
            _hud.Enable();
            
            if (_currentControlledPlayer == null)
                TakeControlOf(_playerTeam[0]);
            
            ObserveBall();
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void ObserveBall()
        {
            _ball.OwnerData.Observe(OnBallOwnerChanged);
        }

        private void OnBallOwnerChanged(Character newOwner)
        {
            switch (newOwner)
            {
                case PlayerFacade player:
                    TakeControlOf(player);
                    break;
            }
        }

        private void TakeControlOf(PlayerFacade player)
        {
           _playerTeam
               .Where(teamMember => teamMember != player)
               .ToArray()
               .Map(GiveUpControlOf);

            _currentControlledPlayer = player;
            _currentControlledPlayer.StateMachine.Enter<ControlledAttackState, Transform>(_sceneConfig.EnemyBascket);
        }

        private void GiveUpControlOf(PlayerFacade player)
        {
            player.StateMachine.Enter<AIControlledState>();
        }
    }
}