using AIR.Fluxity;
using UnityEngine;

namespace Examples.Simple
{
    public static class CounterEffects
    {
        public static void DoIncrementEffect(IncrementCountCommand _1, IDispatcher _2)
        {
            Debug.Log("Increment Command made.");
        }

        public static void DoDecrementEffect(DecrementCountCommand _1, IDispatcher _2)
        {
            Debug.Log("Decrement Command made.");
        }
    }
}