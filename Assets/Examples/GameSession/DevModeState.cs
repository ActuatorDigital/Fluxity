namespace Examples.GameSession
{
    //This is only partially setup so we can demo the use of FeatureObservers in the LivesEffects class
    public struct DevModeState
    {
        public bool InvinciblePlayer;

        public static DevModeState Create() => new() { InvinciblePlayer = false };
    }
}