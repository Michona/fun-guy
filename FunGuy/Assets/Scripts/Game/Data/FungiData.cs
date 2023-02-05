using Util;

namespace Game.Data
{
    public record FungiData(FungiState State = FungiState.Walking, bool IsOnGround = false, bool IsOnRoot = false,
        int Health = GlobalConst.MaxHealth, float LastDamageTimeStamp = 0f)
    {
        public bool CanJump => (IsOnGround || IsOnRoot) && State == FungiState.Walking;

        public bool CanRoot => IsOnGround || IsOnRoot;
    }
}