using AIR.Fluxity;

namespace Examples.GameSession
{
    public static class GameSessionReducers
    {
        public static void RegisterAll(FluxityFeatureContext<GameSessionScoreState> context)
        {
            context
                .Reducer<IncrementScoreCommand>(IncrementScore)
                .Reducer<LockGameSessionCommand>(LockGameSession)
                ;
        }

        public static GameSessionScoreState IncrementScore(GameSessionScoreState state, IncrementScoreCommand command)
        {
            if (!state.Locked)
            {
                state.Score += command.Amount;
            }
            return state;
        }

        public static GameSessionScoreState LockGameSession(GameSessionScoreState state, LockGameSessionCommand command)
        {
            state.Locked = true;
            return state;
        }
    }
}