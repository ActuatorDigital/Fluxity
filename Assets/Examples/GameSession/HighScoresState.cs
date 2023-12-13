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
        public List<HighScoreEntry> HighScoreEntries;

        public static HighScoresState Create() 
            => new HighScoresState
            {
                HighScoreEntries = new List<HighScoreEntry>()
            };
    }
}