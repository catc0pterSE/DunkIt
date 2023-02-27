using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.HUD.Mobile
{
    public class ButtonSimulation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action Down;
        public event Action Up;

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