namespace Examples.Countdown
{
    public struct CountdownState
    {
        public float CountdownDurationSeconds;
        public bool IsRunning;
    }

    public static class CountdownReducer
    {
        public static CountdownState StartCountDown(CountdownState state, StartCountdownCommand command)
        {
            return new CountdownState
            {
                CountdownDurationSeconds = command.Seconds,
                IsRunning = true,
            };
        }

        public static CountdownState StopCountDown(CountdownState state, StopCountdownCommand command)
        {
            return new CountdownState { IsRunning = false };
        }
    }
}