using System.Threading.Tasks;
using AIR.Fluxity;
using UnityEngine;

namespace Examples.Counter
{
    public class IncrementCounterEffect : Effect<IncrementCountCommand>
    {
        public IncrementCounterEffect(IDispatcher dispatcher)
            : base(dispatcher) { }

        public override Task DoEffect(IncrementCountCommand command)
        {
            Debug.Log("Increment Command made.");
            return Task.CompletedTask;
        }
    }
}