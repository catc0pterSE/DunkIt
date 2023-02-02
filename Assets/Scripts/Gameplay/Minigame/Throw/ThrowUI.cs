using Infrastructure.Input;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UI.HUD.Mobile;
using UnityEngine;

namespace Gameplay.Minigame.Throw
{
    public class ThrowUI : SwitchableMonoBehaviour
    {
        [SerializeField] private ButtonSimulation _throwButton;

        private IUIInputController _uiInputController;

        private IUIInputController UIInputController =>
            _uiInputController ??= Services.Container.Single<IUIInputController>();

        private void OnEnable()
        {
            _throwButton.Down += UIInputController.OnUIThrowButtonDown;
            _throwButton.Up += UIInputController.OnUIThrowButtonUp;
        }

        private void OnDisable()
        {
            _throwButton.Down -= UIInputController.OnUIThrowButtonDown;
            _throwButton.Up -= UIInputController.OnUIThrowButtonUp;
        }
    }
}