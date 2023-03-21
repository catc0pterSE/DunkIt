using System;
using System.Collections;
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
           StartCoroutine(InvokeDown());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            StartCoroutine(InvokeUp());
        }

        private IEnumerator InvokeUp()
        {
            yield return new WaitForEndOfFrame();
            Up?.Invoke();
        }

        private IEnumerator InvokeDown()
        {
            yield return new WaitForEndOfFrame();
            Down?.Invoke();
        }
    }
}