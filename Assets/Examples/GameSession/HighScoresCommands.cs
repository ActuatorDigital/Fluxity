using System.Collections.Generic;
using Actuator.Fluxity;

namespace Examples.GameSession
{
    public class HighScoresLoadedCommand : ICommand
    {
        public IReadOnlyList<HighScoreEntry> HighScoreEntries;
    }
}