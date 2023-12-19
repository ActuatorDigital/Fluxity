using AIR.Fluxity;

namespace Examples.Spinner
{
    public class StopSpinCommand : ICommand
    {
    }

    public class StartSpinCommand : ICommand
    {
        public float DegreesPerSecond { get; set; }
    }
}