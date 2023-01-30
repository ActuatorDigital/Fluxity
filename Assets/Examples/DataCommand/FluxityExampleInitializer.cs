using AIR.Fluxity;
using UnityEngine;

namespace Examples.DataCommand
{
    [DefaultExecutionOrder(1)]
    public class FluxityExampleInitializer : FluxityInitializer
    {
        protected override void Initialize()
        {
            CreateReducer<CounterState, ChangeCountCommand>(CounterReducer.Change);

            var changeCounterEffect = new ChangeCounterEffect();
            CreateEffect<ChangeCountCommand>(changeCounterEffect.DoEffect);
        }
    }
}