using System.Reflection;

namespace AIR.Fluxity.Tests.DummyTypes
{
    internal class DummyReducer : Reducer<DummyState, DummyCommand>
    {
        public override DummyState Reduce(DummyState state, DummyCommand command)
        {
            return new DummyState() { value = state.value + command.payload };
        }

        public override MethodInfo ReducerBindingInfo()
        {
            return typeof(DummyReducer).GetMethods()[0];
        }
    }
}