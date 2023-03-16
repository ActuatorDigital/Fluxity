using System.Reflection;
using AIR.Fluxity;

namespace Examples.Simple
{
    public class CounterReducerAlt : Reducer<CounterState, IncrementCountCommand>
    {
        public override CounterState Reduce(CounterState state, IncrementCountCommand command)
        {
            return new CounterState { CurrentCount = state.CurrentCount + 1 };
        }

        public override MethodInfo ReducerBindingInfo()
        {
            return typeof(CounterReducerAlt).GetMethod(nameof(Reduce));
        }
    }
}