using Actuator.Fluxity;

namespace Examples.GameSession
{
    public class SetGamePhaseCommand : ICommand
    {
        public GamePhase GamePhase;
    }
}