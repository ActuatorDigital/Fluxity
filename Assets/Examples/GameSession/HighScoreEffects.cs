using System.Collections.Generic;
using AIR.Fluxity;

namespace Examples.GameSession
{
    public class HighScoreEffects
    {
        internal void HandleGamePhaseChange(SetGamePhaseCommand command, IDispatcher dispatcher)
        {
            if (command.GamePhase == GamePhase.Scores)
            {
                //would need to load from somewhere or hit a service, via a unitask or similar but for
                //the sake of this example we'll just create some dummy data
                var highScoreEntries = new List<HighScoreEntry>
                {
                    new HighScoreEntry {Name = "Player 1", Score = 100},
                    new HighScoreEntry {Name = "Player 2", Score = 200},
                    new HighScoreEntry {Name = "Player 3", Score = 300},
                    new HighScoreEntry {Name = "Player 4", Score = 400},
                    new HighScoreEntry {Name = "Player 5", Score = 500},
                    new HighScoreEntry {Name = "Player 6", Score = 600},
                    new HighScoreEntry {Name = "Player 7", Score = 700},
                    new HighScoreEntry {Name = "Player 8", Score = 800},
                    new HighScoreEntry {Name = "Player 9", Score = 900},
                    new HighScoreEntry {Name = "Player 10", Score = 1000},
                };

                dispatcher.Dispatch(new HighScoresLoadedCommand { HighScoreEntries = highScoreEntries});
            }
        }
    }
}