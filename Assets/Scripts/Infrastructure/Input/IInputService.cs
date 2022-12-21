using Infrastructure.ServiceManagement;
using UnityEngine;

namespace Infrastructure.Input
{
    public interface IInputService : IService
    {
        public Vector2 MovementDirection { get; }
    }
}