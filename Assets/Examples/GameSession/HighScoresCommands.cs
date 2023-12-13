using System.Collections.Generic;
using AIR.Fluxity;

namespace Examples.GameSession
{
    public class HighScoresLoadedCommand : ICommand
    {
        public List<HighScoreEntry> HighScoreEntries;
    }
}