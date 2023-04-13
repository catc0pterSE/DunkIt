﻿using System;
using Infrastructure.Input;
using Modules.MonoBehaviour;
using UI.HUD.Controls.StateMachine;
using UnityEngine;

namespace UI.HUD.Controls.Mobile
{
    public class MobileControlsHUDView : SwitchableMonoBehaviour, IControlsHUDView
    {
        [SerializeField] private ButtonSimulation _throwButton;
        [SerializeField] private ButtonSimulation _dunkButton;
        [SerializeField] private ButtonSimulation _passButton;
        [SerializeField] private ButtonSimulation _changePlayerButton;
        [SerializeField] private GameObject _joystick;


        private IUIInputController _uiInputController;
        private GameplayHUDStateMachine _stateMachine;
        private IControlsHUDStateController _hudStateController;

        private void OnEnable() =>
            SubscribeUIInputControllerOnButtons();

        private void OnDisable() =>
            UnsubscribeUIInputControllerFromButtons();


        public IControlsHUDView Initialize(IControlsHUDStateController controlsHUDStateController)
        {
            _hudStateController = controlsHUDStateController;
            _stateMachine = new GameplayHUDStateMachine(this);
            ObserveStateController();
            return this;
        }

        public void SetUiInputController(IUIInputController uiInputController) =>
            _uiInputController = uiInputController;

        public void SetThrowAvailability(bool isAvailable) =>
            _throwButton.gameObject.SetActive(isAvailable);

        public void SetDunkAvailability(bool isAvailable) =>
            _dunkButton.gameObject.SetActive(isAvailable);

        public void SetPassAvailability(bool isAvailable) =>
            _passButton.gameObject.SetActive(isAvailable);

        public void SetChangePlayerAvailability(bool isAvailable) =>
            _changePlayerButton.gameObject.SetActive(isAvailable);

        public void SetMovementAvailability(bool isAvailable) =>
            _joystick.gameObject.SetActive(isAvailable);

        private void SubscribeUIInputControllerOnButtons()
        {
            _throwButton.Down += _uiInputController.OnUIThrowButtonDown;
            _throwButton.Up += _uiInputController.OnUIThrowButtonUp;

            _dunkButton.Down += _uiInputController.OnUIDunkButtonDown;
            _dunkButton.Up += _uiInputController.OnUIDunkButtonUp;

            _passButton.Down += _uiInputController.OnUIPassButtonDown;
            _passButton.Up += _uiInputController.OnUIPassButtonUp;

            _changePlayerButton.Down += _uiInputController.OnUIChangePlayerButtonDown;
            _changePlayerButton.Up += _uiInputController.OnUIChangePlayerButtonUp;
        }

        private void UnsubscribeUIInputControllerFromButtons()
        {
            _throwButton.Down -= _uiInputController.OnUIThrowButtonDown;
            _throwButton.Up -= _uiInputController.OnUIThrowButtonUp;

            _dunkButton.Down -= _uiInputController.OnUIDunkButtonDown;
            _dunkButton.Up -= _uiInputController.OnUIDunkButtonUp;

            _passButton.Down -= _uiInputController.OnUIPassButtonDown;
            _passButton.Up -= _uiInputController.OnUIPassButtonUp;

            _changePlayerButton.Down -= _uiInputController.OnUIChangePlayerButtonDown;
            _changePlayerButton.Up -= _uiInputController.OnUIChangePlayerButtonUp;
        }

        private void ObserveStateController() =>
            _hudStateController.HudStateSelection.Observe(OnCurrentCurrentPlayerDataChanged);

        private void OnCurrentCurrentPlayerDataChanged(Action<GameplayHUDStateMachine> action) =>
            action.Invoke(_stateMachine);
    }
}