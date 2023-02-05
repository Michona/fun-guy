namespace Util
{
    public static class GlobalConst
    {
        public const string PlayerOne = "0";
        public const string PlayerTwo = "1";


        /* Game */
        public const int MaxHealth = 10;
        public const int InvulnerabilityTime = 2; // seconds

        /* Roots */
        public const int TurnRateAngle = 60; // degrees
        public const float RootSpawnDistanceMultiplier = 0.7f;
        public const float RootSpawnInterval = 0.2f;
        public const float CollisionProximity = 1f; // vector distance
        
        /* Rotation */
        public const float RotationInterval = 5f; // seconds
    }
}