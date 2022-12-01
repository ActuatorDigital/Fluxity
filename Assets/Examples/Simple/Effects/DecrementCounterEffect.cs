using AIR.Fluxity;
using UnityEngine;

namespace Examples.Simple
{
    public class DecrementCounterEffect : Effect<DecrementCountCommand>
    {
        public DecrementCounterEffect(IDispatcher dispatcher)
            : base(dispatcher) { }

        public override void DoEffect(DecrementCountCommand command)
        {
            Debug.Log("Decrement Command made.");
        }
    }
}