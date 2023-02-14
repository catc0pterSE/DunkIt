using System;
using Infrastructure.Input.InputService;
using Infrastructure.ServiceManagement;
using Modules.MonoBehaviour;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Utility.Constants;
using Image = UnityEngine.UI.Image;

namespace Gameplay.Minigame.FightForBall.UI
{
    public class Ball : SwitchableMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;
        [SerializeField] private Image _frame;
        [SerializeField] private Color _heldColor;
        [SerializeField] private Color _unHeldColor;

        private Coroutine _movingRoutine;
        private IInputService _inputService;
        private bool _isPointerOver;
        private bool _isLooping;

        public event Action PointerDown;
        
        private IInputService InputService => _inputService ??= Services.Container.Single<IInputService>();
        //public bool IsHeld => InputService.TouchHeldDown && _isPointerOver;
        public bool IsHeld => Input.GetMouseButton(0) && _isPointerOver;
        public float Offset => _rectTransform.rect.height * NumericConstants.Half;

        private void OnDisable()
        {
            _isPointerOver = false;
        }

        private void Update()
        {
            Color color = IsHeld ? _heldColor : _unHeldColor;
            SetFrameColor(color);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDown?.Invoke();
        }

        private void SetFrameColor(Color color) =>
            _frame.color = color;
    }
}