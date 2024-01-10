using AIR.Flume;
using AIR.Fluxity;
using UnityEngine;

namespace Examples.GameSession
{
    public class FluxityExampleInitializer : FluxityInitializer
    {
        [SerializeField] private ComboRanksSO _comboRanksSO;

        public override void Register(FluxityRegisterContext context)
        {
            var livesEffects = new LivesEffects();

            context
                .Feature(GamePhaseState.Create())
                    .Reducer<SetGamePhaseCommand>(GamePhaseReducers.SetGamePhase)
                .Feature(GameSessionScoreState.Create(), GameSessionReducers.RegisterAll)
                .Feature(HighScoresState.Create())
                    .Reducer<HighScoresLoadedCommand>(AssignHighScores)
                .Feature(ComboRankLookupState.Create(_comboRanksSO.ComboRanks))
                .Feature(LivesState.Create(), LivesReducers.RegisterAll)
                .Feature(DevModeState.Create())

                .Effect(new HighScoreEffects())
                    .Method<SetGamePhaseCommand>(x => x.HandleGamePhaseChange)
                .Effect<PlayerDiedCommand>(livesEffects.HandlePlayerDied)
            ;
        }

        protected override void Install(FlumeServiceContainer container)
        {
        }

        private static HighScoresState AssignHighScores(HighScoresState state, HighScoresLoadedCommand command)
        {
            state.HighScoreEntries = command.HighScoreEntries;
            return state;
        }
    }
}