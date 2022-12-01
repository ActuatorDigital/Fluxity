using AIR.Fluxity;
using UnityEngine;

namespace Examples.Simple
{
    public class IncrementCounterEffect : Effect<IncrementCountCommand>
    {
        public IncrementCounterEffect(IDispatcher dispatcher)
            : base(dispatcher) { }

        public override void DoEffect(IncrementCountCommand command)
        {
            Debug.Log("Increment Command made.");
        }
    }
}