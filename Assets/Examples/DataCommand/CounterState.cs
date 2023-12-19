namespace Examples.DataCommand
{
    public struct CounterState
    {
        public int CurrentCount;
    }

    public static class CounterReducer
    {
        public static CounterState Change(CounterState state, ChangeCountCommand command)
        {
            return new CounterState { CurrentCount = state.CurrentCount + command.Delta };
        }
    }
}