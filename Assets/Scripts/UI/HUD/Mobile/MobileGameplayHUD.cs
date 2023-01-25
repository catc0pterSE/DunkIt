using Infrastructure.Input;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.Mobile
{
    public class MobileGameplayHUD: SwitchableMonoBehaviour, IGameplayHUD
    {
        [SerializeField] private Button _throwButton;
        [SerializeField] private Button _dunkButton;
        [SerializeField] private Button _passButton;

        private IUIInputController _uiInputController;

        private IUIInputController UIInputController =>
            _uiInputController ??= Services.Container.Single<IUIInputController>();

        private void OnEnable()
        {
            _throwButton.onClick.AddListener(UIInputController.OnUIThrowButtonClicked);
            _dunkButton.onClick.AddListener(UIInputController.OnUIDunkButtonClicked);
            _passButton.onClick.AddListener(UIInputController.OnUIPassButtonClicked);
        }

        private void OnDisable()
        {
            _throwButton.onClick.RemoveListener(UIInputController.OnUIThrowButtonClicked);
            _dunkButton.onClick.RemoveListener(UIInputController.OnUIDunkButtonClicked);
            _passButton.onClick.RemoveListener(UIInputController.OnUIPassButtonClicked);
        }
    }
}