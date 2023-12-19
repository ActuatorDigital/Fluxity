using System.Collections.Generic;

namespace Examples.GameSession
{
    public struct HighScoreEntry
    {
        public string Name;
        public int Score;
    }

    public struct HighScoresState
    {
        public IReadOnlyList<HighScoreEntry> HighScoreEntries;

        public static HighScoresState Create() 
            => new()
            {
                HighScoreEntries = new List<HighScoreEntry>()
            };
    }
}