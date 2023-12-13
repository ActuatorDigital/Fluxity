using AIR.Fluxity;

namespace Examples.GameSession
{
    public static class GamePhaseReducers
    {
        //could use the same semantic and bulk reduce 1 thing
        //public static void RegisterAll(FluxityFeatureContext<GamePhaseState> context)
        //{
        //    context
        //        .Reducer<SetGamePhaseCommand>(SetGamePhase)
        //        ;
        //}

        internal static GamePhaseState SetGamePhase(GamePhaseState state, SetGamePhaseCommand command)
        {
            state.GamePhase = command.GamePhase;
            return state;
        }
    }
}