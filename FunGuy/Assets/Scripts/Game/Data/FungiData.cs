namespace Game.Data
{
    public record FungiData(FungiState State, bool IsOnGround, bool IsOnRoot)
    {
        public bool CanJump => (IsOnGround || IsOnRoot) && State == FungiState.Walking;

        public bool CanRoot => IsOnGround || IsOnRoot;
    }
}