namespace Examples.Counter
{
    public static class CounterReducer
    {
        public static CounterState Increment(CounterState state, IncrementCountCommand command)
        {
            return new CounterState { CurrentCount = state.CurrentCount + 1 };
        }

        public static CounterState Decrement(CounterState state, DecrementCountCommand command)
        {
            return new CounterState { CurrentCount = state.CurrentCount - 1 };
        }
    }
}