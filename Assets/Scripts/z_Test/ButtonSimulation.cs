using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace z_Test
{
    public class ButtonSimulation: MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        public bool IsDown { get; private set; }
        public bool IsUp { get; private set; }
        public bool Clicked { get; private set; }

        private Coroutine ResetingClick;
        private Coroutine ResetingPointerUP;
        
        private void OnEnable()
        {
            IsDown = false;
            IsUp = false;
            Clicked = false;
        }

        private void OnDisable()
        {
            IsDown = false;
            IsUp = false;
            Clicked = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsDown = false;
            IsUp = true;

            ResetingPointerUP = StartCoroutine(ResetIsUp());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked = true;
            
            ResetingClick = StartCoroutine(ResetClicked());
        }

        private IEnumerator ResetIsUp()
        {
            yield return null;
            IsUp = false;
        }
        
        private IEnumerator ResetClicked()
        {
            yield return null;
            Clicked = false;
        }
    }
}