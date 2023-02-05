using UnityEngine;

namespace Roots.Data
{
    public record Joint(Vector2 Position, int ID, Vector2 Direction, int? Prev = null, int? Next = null);
}