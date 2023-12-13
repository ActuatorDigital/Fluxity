using AIR.Fluxity;
using UnityEngine;

namespace Examples.GameSession
{
    [DefaultExecutionOrder(1)]
    public class FluxityExampleInitializer : FluxityInitializer
    {
        public static void Setup(FluxityFlumeRegisterContext context)
        {
            context
                .Feature(GamePhaseState.Create())
                    .Reducer<SetGamePhaseCommand>(GamePhaseReducers.SetGamePhase)
                .Feature(GameSessionScoreState.Create(), GameSessionReducers.RegisterAll)
                .Feature(HighScoresState.Create())
                    .Reducer<HighScoresLoadedCommand>(AssignHighScores)
                ;
        }

        private static HighScoresState AssignHighScores(HighScoresState state, HighScoresLoadedCommand command)
        {
            state.HighScoreEntries = command.HighScoreEntries;
            return state;
        }

        protected override void Initialize()
        {
            var highScoreEffects = new HighScoreEffects();

            CreateEffect<SetGamePhaseCommand>(highScoreEffects.HandleGamePhaseChange);
        }
    }
}