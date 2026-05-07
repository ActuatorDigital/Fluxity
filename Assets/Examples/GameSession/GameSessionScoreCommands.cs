using Actuator.Fluxity;

namespace Examples.GameSession
{
    public class IncrementScoreCommand : ICommand
    {
        public int Amount;
    }

    public class LockGameSessionCommand : ICommand
    {
    }
}