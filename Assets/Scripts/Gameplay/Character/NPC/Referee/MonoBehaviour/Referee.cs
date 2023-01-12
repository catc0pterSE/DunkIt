﻿using UnityEngine;

namespace Gameplay.NPC.Referee.MonoBehaviour
{
    public class Referee : UnityEngine.MonoBehaviour
    {
        [SerializeField] private Transform _ballPosition;
        
        public Transform BallPosition => _ballPosition;
    }
}