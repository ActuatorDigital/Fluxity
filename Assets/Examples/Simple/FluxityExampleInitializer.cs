using AIR.Fluxity;

namespace Examples.Counter
{
    public class FluxityExampleInitializer : FluxityInitializer
    {
        protected override void Initialize()
        {
            CreateReducer<CounterState, IncrementCountCommand>(CounterReducer.Increment);
            CreateReducer<CounterState, DecrementCountCommand>(CounterReducer.Decrement);

            CreateEffect<IncrementCounterEffect, IncrementCountCommand>();
            CreateEffect<DecrementCounterEffect, DecrementCountCommand>();
        }
    }
}