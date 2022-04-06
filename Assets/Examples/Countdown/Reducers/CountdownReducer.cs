namespace Examples.Countdown
{
    public static class CountdownReducer
    {
        public static CountdownState StartCountDown(CountdownState state, StartCountdownCommand command)
        {
            return new CountdownState {
                CountdownSeconds = command.Seconds,
                IsRunning = true,
            };
        }

        public static CountdownState StopCountDown(CountdownState state, StopCountdownCommand command)
        {
            return new CountdownState { IsRunning = false };
        }
    }
}