using Gameplay.Character.Player.MonoBehaviour;
using Infrastructure.Input;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using UI.HUD.StateMachine;
using UI.Indication;
using UnityEngine;
using z_Test;

namespace UI.HUD.Mobile
{
    public class MobileGameplayHUD : SwitchableMonoBehaviour, IGameplayHUD
    {
        [SerializeField] private ButtonSimulation _throwButton;
        [SerializeField] private ButtonSimulation _dunkButton;
        [SerializeField] private ButtonSimulation _passButton;
        [SerializeField] private ButtonSimulation _changePlayerButton;
        [SerializeField] private OffScreenIndicationRenderer _offScreenIndicationRenderer;

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
            UnsubscribeUIInputControllerFromButtons();
        }

        public IGameplayHUD Initialize(PlayerFacade[] indicationTargets, Camera gameplayCamera)
        {
            _offScreenIndicationRenderer.Initialize(indicationTargets, gameplayCamera);
            return this;
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
            _throwButton.Down += UIInputController.OnUIThrowButtonDown;
            _throwButton.Up += UIInputController.OnUIThrowButtonUp;

            _dunkButton.Down += UIInputController.OnUIDunkButtonDown;
            _dunkButton.Up += UIInputController.OnUIDunkButtonUp;

            _passButton.Down += UIInputController.OnUIPassButtonDown;
            _passButton.Up += UIInputController.OnUIPassButtonUp;

            _changePlayerButton.Down += UIInputController.OnUIChangePlayerButtonDown;
            _changePlayerButton.Up += UIInputController.OnUIChangePlayerButtonUp;
        }

        private void UnsubscribeUIInputControllerFromButtons()
        {
            _throwButton.Down -= UIInputController.OnUIThrowButtonDown;
            _throwButton.Up -= UIInputController.OnUIThrowButtonUp;

            _dunkButton.Down -= UIInputController.OnUIDunkButtonDown;
            _dunkButton.Up -= UIInputController.OnUIDunkButtonUp;

            _passButton.Down -= UIInputController.OnUIPassButtonDown;
            _passButton.Up -= UIInputController.OnUIPassButtonUp;

            _changePlayerButton.Down -= UIInputController.OnUIChangePlayerButtonDown;
            _changePlayerButton.Up -= UIInputController.OnUIChangePlayerButtonUp;
        }
    }
}