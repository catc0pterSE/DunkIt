using System;
using Gameplay.Character;
using Infrastructure.CoroutineRunner;
using Modules.MonoBehaviour;
using Scene;
using UI;
using UnityEngine;

namespace Gameplay.Minigame.Throw
{
    using Ball.MonoBehavior;
    public class ThrowMinigame : SwitchableMonoBehaviour, IMinigame
    {
        [SerializeField] private ThrowUI _interface;

        public event Action Won;
        public event Action Lost;

        public void Initialize
        (
            BasketballPlayer throwingPlayer,
            BasketballPlayer otherTeamPlayer,
            SceneConfig sceneConfig,
            Ball ball,
            LoadingCurtain loadingCurtain
        )
        {
        }

        public void Launch()
        {
            _interface.Enable();
        }
    }
}