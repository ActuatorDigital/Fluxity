using AIR.Fluxity;

namespace Examples.DataCommand
{
    public struct CounterState
    {
        public int CurrentCount;
    }

    public class ChangeCountCommand : ICommand
    {
        public int Delta { get; set; }
    }

    public static class CounterReducer
    {
        public static CounterState Change(CounterState state, ChangeCountCommand command)
        {
            state.CurrentCount += command.Delta;
            return state;
        }
    }
}