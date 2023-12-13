namespace Examples.GameSession
{
    public struct GameSessionScoreState
    {
        public bool Locked;
        public int Score;

        public static GameSessionScoreState Create() 
            => new GameSessionScoreState 
            {
                Locked = false,
                Score = 0 
            };
    }
}