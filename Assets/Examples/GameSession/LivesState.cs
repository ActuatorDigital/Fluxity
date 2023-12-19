using System;
using AIR.Fluxity;

namespace Examples.GameSession
{
    public struct LivesState
    {
        public int lives;

        public static LivesState Create() => new() { lives = 3 };
    }

    public static class LivesReducers
    {
        internal static void RegisterAll(FluxityFeatureContext<LivesState> context)
        {
            context
                .Reducer<SetLivesCommand>(SetLives)
                .Reducer<PlayerDiedCommand>(PlayerDied)
                ;
        }

        internal static LivesState SetLives(LivesState state, SetLivesCommand command)
        {
            state.lives = command.lives;
            return state;
        }

        internal static LivesState PlayerDied(LivesState state, PlayerDiedCommand command)
        {
            state.lives--;
            return state;
        }
    }

    public class SetLivesCommand : ICommand
    {
        public int lives;
    }

    public class PlayerDiedCommand : ICommand
    {
    }

    public class GameOverCommand : ICommand
    {
    }
}