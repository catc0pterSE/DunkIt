using Infrastructure.Input;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UI.HUD.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD.Mobile
{
    public class MobileGameplayHUD : SwitchableMonoBehaviour, IGameplayHUD
    {
        [SerializeField] private Button _throwButton;
        [SerializeField] private Button _dunkButton;
        [SerializeField] private Button _passButton;
        [SerializeField] private Button _changePlayerButton;

        private IUIInputController _uiInputController;
        private GameplayHUDStateMachine _stateMachine;

        private IUIInputController UIInputController =>
            _uiInputController ??= Services.Container.Single<IUIInputController>();

        public GameplayHUDStateMachine StateMachine => _stateMachine ??= new GameplayHUDStateMachine(this);

        private void OnEnable()
        {
            SubscribeUIInputControllerOnButtons();
        }

        private void OnDisable()
        {
            UnsubscribeUIInputControllerOnButtons();
        }

        public void SetThrowAvailability(bool isAvailable) =>
            _throwButton.gameObject.SetActive(isAvailable);

        public void SetDunkAvailability(bool isAvailable) =>
            _dunkButton.gameObject.SetActive(isAvailable);

        public void SetPassAvailability(bool isAvailable) =>
            _passButton.gameObject.SetActive(isAvailable);

        public void SetChangePlayerAvailability(bool isAvailable) =>
            _changePlayerButton.gameObject.SetActive(isAvailable);
        
        private void SubscribeUIInputControllerOnButtons()
        {
            _throwButton.onClick.AddListener(UIInputController.OnUIThrowButtonClicked);
            _dunkButton.onClick.AddListener(UIInputController.OnUIDunkButtonClicked);
            _passButton.onClick.AddListener(UIInputController.OnUIPassButtonClicked);
            _changePlayerButton.onClick.AddListener(UIInputController.OnUIChangePlayerButtonClicked);
        }

        private void UnsubscribeUIInputControllerOnButtons()
        {
            _throwButton.onClick.RemoveListener(UIInputController.OnUIThrowButtonClicked);
            _dunkButton.onClick.RemoveListener(UIInputController.OnUIDunkButtonClicked);
            _passButton.onClick.RemoveListener(UIInputController.OnUIPassButtonClicked);
            _changePlayerButton.onClick.AddListener(UIInputController.OnUIChangePlayerButtonClicked);
        }
    }
}