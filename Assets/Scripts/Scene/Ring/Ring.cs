using System;
using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scene.Ring
{
    public class Ring : MonoBehaviour
    {
        [SerializeField] private float _goalTrackingWindowTime = 3f;
        [SerializeField] private RingCap _cap;
        [SerializeField] private WinZone _winZone;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Transform[] _dunkPoints;
        [SerializeField] private Transform _ballDunkPoint;
        [SerializeField] private CinemachineVirtualCamera[] _dunkVirtualCameras;
        
        private Coroutine _listeningToWinZone;
        private WaitForSeconds _goalTrackingWindowWait;
        public event Action Goal;

        public Transform[] DunkPoints => _dunkPoints.ToArray();

        public Transform BallDunkPoint => _ballDunkPoint;

        public CinemachineVirtualCamera VirtualCamera => _virtualCamera;

        public CinemachineVirtualCamera DunkVirtualCamera =>
            _dunkVirtualCameras[Random.Range(0, _dunkVirtualCameras.Length)];

        private void Awake()
        {
            _goalTrackingWindowWait = new WaitForSeconds(_goalTrackingWindowTime);
        }

        private void OnEnable()
        {
            _cap.Entered += OnCapEntered;
        }

        private void OnDisable()
        {
            _cap.Entered -= OnCapEntered;
        }

        private void OnCapEntered()
        {
            if (_listeningToWinZone != null)
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
            if (_listeningToWinZone != null)
                StopCoroutine(_listeningToWinZone);

            Goal?.Invoke();
        }
    }
}