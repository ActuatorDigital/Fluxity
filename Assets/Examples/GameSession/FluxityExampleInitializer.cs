using AIR.Flume;
using AIR.Fluxity;
using System.Collections.Generic;
using UnityEngine;

namespace Examples.GameSession
{
    public class FluxityExampleInitializer : FluxityInitializer
    {
        [SerializeField] private ComboRanksSO _comboRanksSO;

        public override void RegisterFluxity(FluxityRegisterContext context)
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

                .OnPostDispatch<PlayerDiedCommand>(HandlePlayerDied)
            ;
        }

        protected override void RegisterServices(FlumeServiceContainer container)
        {
        }

        protected override void PostInitialize(IDispatcher dispatcher)
        {
            var highScoreEntries = new List<HighScoreEntry>
                {
                    new() {Name = "Player 1", Score = 100},
                    new() {Name = "Player 2", Score = 200},
                    new() {Name = "Player 3", Score = 300},
                    new() {Name = "Player 4", Score = 400},
                    new() {Name = "Player 5", Score = 500},
                    new() {Name = "Player 6", Score = 600},
                    new() {Name = "Player 7", Score = 700},
                    new() {Name = "Player 8", Score = 800},
                    new() {Name = "Player 9", Score = 900},
                    new() {Name = "Player 10", Score = 1000},
                };

            dispatcher.Dispatch(new HighScoresLoadedCommand { HighScoreEntries = highScoreEntries });
        }

        private static HighScoresState AssignHighScores(HighScoresState state, HighScoresLoadedCommand command)
        {
            state.HighScoreEntries = command.HighScoreEntries;
            return state;
        }

        private readonly FeatureHandle<LivesState> _livesState = new();
        private readonly FeatureHandle<DevModeState> _devMode = new();

        public void HandlePlayerDied(PlayerDiedCommand cmd)
        {
            if (_livesState.State.lives <= 0
                && !_devMode.State.InvinciblePlayer)
            {
                new DispatcherHandle().Dispatch(new GameOverCommand());
            }
        }
    }
}