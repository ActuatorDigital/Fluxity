using AIR.Fluxity;
using UnityEngine;

namespace Examples.Simple
{
    [DefaultExecutionOrder(1)]
    public class FluxityExampleInitializer : FluxityInitializer
    {
        protected override void Initialize()
        {
            CreateReducer<CounterState, IncrementCountCommand>(CounterReducer.Increment);
            CreateReducer<CounterState, DecrementCountCommand>(CounterReducer.Decrement);

            // NOTE: You don't need to make an effect for every command, we're doing so here
            // purely for example purposes. In this case it's a static class with many effect methods
            CreateEffect<IncrementCountCommand>(CounterEffects.DoIncrementEffect);
            CreateEffect<DecrementCountCommand>(CounterEffects.DoDecrementEffect);
        }
    }
}