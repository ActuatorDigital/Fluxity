using AIR.Fluxity;
using UnityEngine;

namespace Examples.GameSession
{
    [DefaultExecutionOrder(1)]
    public class FluxityExampleInitializer : FluxityInitializer
    {
        [SerializeField] private ComboRanksSO _comboRanksSO;

        public void Setup(FluxityFlumeRegisterContext context)
        {
            context
                .Feature(GamePhaseState.Create())
                    .Reducer<SetGamePhaseCommand>(GamePhaseReducers.SetGamePhase)
                .Feature(GameSessionScoreState.Create(), GameSessionReducers.RegisterAll)
                .Feature(HighScoresState.Create())
                    .Reducer<HighScoresLoadedCommand>(AssignHighScores)
                .Feature(ComboRankLookupState.Create(_comboRanksSO.ComboRanks))
                .Feature(LivesState.Create(), LivesReducers.RegisterAll)
                .Feature(DevModeState.Create())
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
            var livesEffects = new LivesEffects();

            CreateEffect<SetGamePhaseCommand>(highScoreEffects.HandleGamePhaseChange);
            CreateEffect<PlayerDiedCommand>(livesEffects.HandlePlayerDied);
        }
    }
}