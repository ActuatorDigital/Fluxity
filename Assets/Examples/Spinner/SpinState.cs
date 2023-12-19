namespace Examples.Spinner
{
    public struct SpinState
    {
        public bool DoSpin;
        public float DegreesPerSecond;
    }

    public static class SpinnerReducers
    {
        public static SpinState StartSpin(SpinState state, StartSpinCommand command)
        {
            return new SpinState
            {
                DegreesPerSecond = command.DegreesPerSecond,
                DoSpin = true,
            };
        }

        public static SpinState StopSpin(SpinState state, StopSpinCommand command)
        {
            return new SpinState { DoSpin = false };
        }
    }
}