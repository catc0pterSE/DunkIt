using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.HUD.Mobile
{
    public class ButtonSimulation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action Down;
        public event Action Up;

        private void OnDisable()
        {
            Up?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Down?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Up?.Invoke();
        }
    }
}