using AIR.Fluxity;

namespace Examples.Simple
{
    public struct CounterState
    {
        public int CurrentCount;
    }

    public static class CounterReducer
    {
        public static void RegisterAll(FluxityRegisterFeatureContext<CounterState> fluxityFeatureContext)
        {
            fluxityFeatureContext
                .Reducer<IncrementCountCommand>(Increment)
                .Reducer<DecrementCountCommand>(Decrement);
        }

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