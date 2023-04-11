using System.Collections.Generic;
using Gameplay.Character.Player.MonoBehaviour;
using UnityEngine;
using UnityEngine.UI;
using Utility.Constants;
using Utility.Extensions;

namespace UI.Indication
{
    public class OffScreenIndicationRenderer : MonoBehaviour
    {
        [SerializeField] private Color _allyColor;
        [SerializeField] private Color _enemyColor;
        [SerializeField] private float _offset = 10;
        [SerializeField] private Image[] _indicators = new Image[NumericConstants.PlayersInTeam];

        private Camera _camera;
        private PlayerFacade[] _targets;
        private Dictionary<PlayerFacade, Image> _dedicatedIndicators = new Dictionary<PlayerFacade, Image>();

        private void Update()
        {
            Render();
        }

        public void Initialize(PlayerFacade[] targets, Camera gamePlayCamera)
        {
            _targets = targets;
            _camera = gamePlayCamera;

            for (int i = 0; i < targets.Length; i++)
            {
                Image indicator = _indicators[i];
                PlayerFacade target = targets[i];
                indicator.color = target.CanBeLocalControlled ? _allyColor : _enemyColor;
                _dedicatedIndicators[target] = indicator;
            }
        }

        private void Render()
        {
            foreach (KeyValuePair<PlayerFacade, Image> pair in _dedicatedIndicators)
            {
                PlayerFacade target = pair.Key;
                Image indicator = pair.Value;

                Vector3 screenPosition = _camera.WorldToScreenPoint(target.transform.position);

                bool targetIsInScreenSpace =
                    screenPosition.z > 0
                    && screenPosition.x > 0 && screenPosition.x < Screen.width
                    && screenPosition.y > 0 && screenPosition.y < Screen.height;

                if (targetIsInScreenSpace)
                {
                    indicator.gameObject.SetActive(false);
                }
                else
                {
                    indicator.gameObject.SetActive(true);
                    Vector3 indicatorPosition = GetIndicatorPosition(screenPosition);
                    indicator.rectTransform.position = indicatorPosition;
                    RotateIndicator(indicator);
                }
            }
        }

        private Vector3 GetIndicatorPosition(Vector3 roughPosition)
        {
            bool isBehindScreen = roughPosition.z < 0;

            if (isBehindScreen)
                roughPosition = GetMirroredPosition(roughPosition);

            float positionX = Mathf.Clamp(roughPosition.x, _offset, Screen.width - _offset);
            float positionY = Mathf.Clamp(roughPosition.y, _offset, Screen.height - _offset);

            return new Vector3(positionX, positionY, 0);
        }

        private Vector3 GetMirroredPosition(Vector3 roughPosition) =>
            new Vector3(
                Screen.width - roughPosition.x,
                Screen.height - roughPosition.y,
                roughPosition.z);

        private void RotateIndicator(Image indicator)
        {
            Vector3 screenCenter = new Vector3(Screen.width*NumericConstants.Half, Screen.height*NumericConstants.Half);
            Vector3 direction = (indicator.rectTransform.position - screenCenter).normalized;

            indicator.transform.right = -direction;
        }
    }
}