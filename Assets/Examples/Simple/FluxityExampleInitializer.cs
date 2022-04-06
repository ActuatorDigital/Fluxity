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
            // purely for example purposes.
            CreateEffect<IncrementCounterEffect, IncrementCountCommand>();
            CreateEffect<DecrementCounterEffect, DecrementCountCommand>();
        }
    }
}