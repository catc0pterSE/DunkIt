using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Scene.Ring
{
    public class Ring: MonoBehaviour
    {
        [SerializeField] private float _goalTrackingWindowTime = 1f;
        [SerializeField] private RingCap _cap;
        [SerializeField] private WinZone _winZone;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        private Coroutine _listeningToWinZone;
        private WaitForSeconds _goalTrackingWindowWait;
        public event Action Goal;

        public CinemachineVirtualCamera VirtualCamera => _virtualCamera;

        private void Awake()
        {
            _goalTrackingWindowWait = new WaitForSeconds(_goalTrackingWindowTime);
        }

        private void OnEnable()
        {
            _cap.Pierced += OnCapPierced;
        }

        private void OnDisable()
        {
            _cap.Pierced -= OnCapPierced;
        }

        private void OnCapPierced()
        {
            if (_listeningToWinZone!=null)
                StopCoroutine(_listeningToWinZone);
            StartCoroutine(ListenToWinZone());
        }

        private IEnumerator ListenToWinZone()
        {
            _winZone.Entered += ScoreGoal;
            yield return _goalTrackingWindowWait;
            _winZone.Entered -= ScoreGoal;
        }

        private void ScoreGoal()
        {
            Goal?.Invoke();
        }
    }
}