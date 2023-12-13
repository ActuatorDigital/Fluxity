namespace Examples.GameSession
{
    public enum GamePhase
    {
        Intro,
        Play,
        Scores,
        Outro,
    }

    public struct GamePhaseState
    {
        public GamePhase GamePhase;

        public static GamePhaseState Create() => new GamePhaseState { GamePhase = GamePhase.Intro };
    }
}