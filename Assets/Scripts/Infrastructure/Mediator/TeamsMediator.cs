using System;
using System.Linq;
using Gameplay.Ball.MonoBehavior;
using Gameplay.Camera;
using Gameplay.Character.Player.MonoBehaviour;
using Infrastructure.Input.InputService;
using Infrastructure.PlayerService;
using Scene;
using Scene.Ring;
using UnityEngine;
using Utility.Constants;

namespace Infrastructure.Mediator
{
    public class TeamsMediator
    {
        private readonly PlayerFacade[] _leftTeam;
        private readonly PlayerFacade[] _rightTeam;
        private readonly PlayerFacade[] _inputControlledTeam;
        private readonly PlayerFacade[] _aiControlledTeam;
        private readonly Ball _ball;
        private readonly CameraFacade _camera;
        private readonly IInputService _inputService;
        private readonly IPlayerService _playerService;
        private readonly Ring _leftRing;
        private readonly Ring _rightRing;
        private readonly CourtDimensions _courtDimensions;

        public TeamsMediator
        (
            PlayerFacade[] leftTeam,
            PlayerFacade[] rightTeam,
            Ball ball,
            CameraFacade camera,
            SceneInitials sceneInitials,
            IInputService inputService,
            IPlayerService playerService
        )
        {
            _leftTeam = leftTeam;
            _rightTeam = rightTeam;
            _ball = ball;
            _camera = camera;
            _inputService = inputService;
            _playerService = playerService;
            _leftRing = sceneInitials.LeftRing;
            _rightRing = sceneInitials.RightRing;
            _courtDimensions = sceneInitials.CourtDimensions;
        }

        public PlayerFacade[] GetOppositeTeamPlayers(PlayerFacade requester)
        {
            if (_leftTeam.Contains(requester))
                return _rightTeam.ToArray();

            if (_rightTeam.Contains(requester))
                return _leftTeam.ToArray();

            throw new ArgumentException("Requester is not part of any team");
        }

        public PlayerFacade[] GetAllies(PlayerFacade requester)
        {
            if (_leftTeam.Contains(requester))
                return _leftTeam.Where(player => player != requester).ToArray();

            if (_rightTeam.Contains(requester))
                return _rightTeam.Where(player => player != requester).ToArray();

            throw new ArgumentException("Requester is not part of any team");
        }

        public Ring GetPlayersRing(PlayerFacade requester)
        {
            if (_leftTeam.Contains(requester))
                return _leftRing;

            if (_rightTeam.Contains(requester))
                return _rightRing;

            throw new ArgumentException("Requester is not part of any team");
        }

        public Ring GetOppositeRing(PlayerFacade requester)
        {
            if (_leftTeam.Contains(requester))
                return _rightRing;

            if (_rightTeam.Contains(requester))
                return _leftRing;

            throw new ArgumentException("Requester is not part of any team");
        }

        public Ball GetBall() => _ball;
        public Camera GetCamera() => _camera.Camera;
        public IInputService GetInputService() => _inputService;
        public IPlayerService GetPlayerService() => _playerService;
        public CourtDimensions GetCourtDimensions() => _courtDimensions;

        public bool TryGetNextToControl(PlayerFacade requester, out PlayerFacade nextPlayer)
        {
            nextPlayer = null;
            PlayerFacade[] playersTeam;

            if (_leftTeam.Contains(requester))
                playersTeam = _leftTeam;
            else if (_rightTeam.Contains(requester))
                playersTeam = _rightTeam;
            else
                throw new ArgumentException("Requester is not part of any team");

            for (int i = 0; i < playersTeam.Length; i++)
            {
                if (playersTeam[i]!=requester)
                    continue;

                if (i == playersTeam.Length-1)
                    nextPlayer = playersTeam[0];
                else
                    nextPlayer = playersTeam[i + 1];
            }

            return nextPlayer != null;
        }
    }
}