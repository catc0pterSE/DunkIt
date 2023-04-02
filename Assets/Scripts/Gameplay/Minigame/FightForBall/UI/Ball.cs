using System;
using Infrastructure.Input.InputService;
using Modules.MonoBehaviour;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility.Constants;

namespace Gameplay.Minigame.FightForBall.UI
{
    public class Ball : SwitchableMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private FightForBallUI _fightForBallUI;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _frame;
        [SerializeField] private Color _heldColor;
        [SerializeField] private Color _unHeldColor;
        [SerializeField] private Color _caughtColor;

        private Coroutine _movingRoutine;
        private IInputService _inputService;
        private bool _isLooping;

        public float ScaledOffset => _rectTransform.rect.height * NumericConstants.Half * ScaleModifier;
        private float ScaleModifier => _fightForBallUI.ScaleModifier;

        public event Action Touched;
        private bool _isCaught;

        private void OnEnable()
        {
            _isCaught = false;
            transform.localScale = _fightForBallUI.Scale;
        }
        
        private void OnDisable()
        {
            transform.localScale = Vector3.one;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isCaught)
                return;;
            
            Touched?.Invoke();
            SetFrameColor(_heldColor);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isCaught)
                return;;
            
            Vector3 pointerPosition = eventData.position;

            Vector3 clampedPointerPosition = new Vector3
            (
                Mathf.Clamp(pointerPosition.x, ScaledOffset, Screen.width - ScaledOffset),
                Mathf.Clamp(pointerPosition.y, ScaledOffset, Screen.height - ScaledOffset)
            );

            transform.position = clampedPointerPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isCaught)
                return;;
            
            SetFrameColor(_unHeldColor);
        }

        public void Stop()
        {
            _isCaught = true;
            SetFrameColor(_caughtColor);
        }
            

        private void SetFrameColor(Color color) =>
            _frame.color = color;
    }
}