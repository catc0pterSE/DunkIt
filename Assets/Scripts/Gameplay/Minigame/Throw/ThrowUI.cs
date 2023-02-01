using System;
using Infrastructure.Input;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Minigame.Throw
{
    public class ThrowUI : SwitchableMonoBehaviour
    {
        [SerializeField] private Button _throwButton;

        private IUIInputController _uiInputController;

        private IUIInputController UIInputController =>
            _uiInputController ??= Services.Container.Single<IUIInputController>();

        private void OnEnable()
        {
            SubscribeUIInputControllerOnThrowButton();
        }

        private void OnDisable()
        {
            UnsubscribeUIInputControllerFromThrowButton();
        }

        private void SubscribeUIInputControllerOnThrowButton() =>
            _throwButton.onClick.AddListener(UIInputController.OnUIThrowButtonClicked);

        private void UnsubscribeUIInputControllerFromThrowButton() =>
            _throwButton.onClick.RemoveListener(UIInputController.OnUIThrowButtonClicked);
    }
}