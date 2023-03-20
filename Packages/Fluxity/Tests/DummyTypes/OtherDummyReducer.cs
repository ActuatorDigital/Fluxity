using System.Reflection;

namespace AIR.Fluxity.Tests.DummyTypes
{
    internal class OtherDummyReducer : Reducer<OtherDummyState, OtherDummyCommand>
    {
        public override OtherDummyState Reduce(OtherDummyState state, OtherDummyCommand command)
        {
            return new OtherDummyState();
        }

        public override MethodInfo ReducerBindingInfo()
        {
            throw new System.NotImplementedException();
        }
    }
}
