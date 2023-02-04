using UnityEngine;

namespace Roots.Data
{
    public record Joint(Vector2 Position, int ID, int? Prev = null, int? Next = null);
}