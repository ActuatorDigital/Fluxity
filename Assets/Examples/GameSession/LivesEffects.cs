using AIR.Fluxity;

namespace Examples.GameSession
{
    //This is only partially set up, but shows how you can use FeatureObservers to gain
    //  access to other feature from elsewhere in your code, at will as needed.
    public class LivesEffects
    {
        private readonly FeatureObserver<LivesState> _livesState = new();
        private readonly FeatureObserver<DevModeState> _devMode = new();

        public void HandlePlayerDied(PlayerDiedCommand _, IDispatcher dispatcher)
        {
            if (_livesState.State.lives <= 0 
                && !_devMode.State.InvinciblePlayer)
            {
                dispatcher.Dispatch(new GameOverCommand());
            }
        }
    }
}