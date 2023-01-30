using AIR.Fluxity;
using UnityEngine;

namespace Examples.Simple
{
    public static class CounterEffects
    {
        public static void DoIncrementEffect(IncrementCountCommand command, IDispatcher dispatcher)
        {
            Debug.Log("Increment Command made.");
        }

        public static void DoDecrementEffect(DecrementCountCommand command, IDispatcher dispatcher)
        {
            Debug.Log("Decrement Command made.");
        }
    }
}