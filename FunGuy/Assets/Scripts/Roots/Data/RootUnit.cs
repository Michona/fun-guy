using UnityEngine;

namespace Roots.Data
{
    public record RootUnit(Vector2 Position, Quaternion Rotation, string PID)
    {
        public Vector2 Position { get; } = Position;
        public Quaternion Rotation { get; } = Rotation;
    }
}