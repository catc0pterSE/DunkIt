using UnityEngine;

namespace Gameplay.Character.NPC.Referee.MonoBehaviour
{
    public class Referee : UnityEngine.MonoBehaviour
    {
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private Animator _animator;
        
        public Transform BallPosition => _ballPosition;
        public Animator Animator => _animator;
    }
}