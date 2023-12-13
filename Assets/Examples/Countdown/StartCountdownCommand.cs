using AIR.Fluxity;

namespace Examples.Countdown
{
    public class StartCountdownCommand : ICommand
    {
        public float Seconds { get; set; }
    }
}