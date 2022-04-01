using AIR.Fluxity;

namespace Examples.Counter
{
    public class CounterReducerAlt : Reducer<CounterState, IncrementCountCommand>
    {
        public override CounterState Reduce(CounterState state, IncrementCountCommand command)
        {
            return new CounterState { CurrentCount = state.CurrentCount + 1 };
        }
    }
}