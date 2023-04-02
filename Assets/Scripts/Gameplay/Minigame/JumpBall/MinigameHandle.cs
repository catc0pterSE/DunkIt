using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.Minigame.JumpBall
{
    public class MinigameHandle : MonoBehaviour
    {
        [SerializeField] private RectTransform _topRaycastPoint;
        [SerializeField] private RectTransform _bottomRaycastPoint;
        [SerializeField] private Color _validColor;
        [SerializeField] private Color _inValidColor;
        [SerializeField] private Image _image;
        [SerializeField] private GraphicRaycaster _raycaster;

        public bool IsInZone => Raycast(_topRaycastPoint.position) && Raycast(_bottomRaycastPoint.position);

        private void Update()
        {
            SetColor(IsInZone ? _validColor : _inValidColor);
        }

        private void SetColor(Color color) =>
            _image.color = color;

        private bool Raycast(Vector3 position)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = position;
            List<RaycastResult> results = new List<RaycastResult>();
            
            _raycaster.Raycast(pointerEventData, results);

            return results.Count > 0;
        }
    }
}