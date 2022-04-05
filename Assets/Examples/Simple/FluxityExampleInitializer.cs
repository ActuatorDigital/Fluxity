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

            CreateEffect<IncrementCounterEffect, IncrementCountCommand>();
            CreateEffect<DecrementCounterEffect, DecrementCountCommand>();
        }
    }
}