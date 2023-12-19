namespace AIR.Fluxity.Tests.DummyTypes
{
    public struct DummyState
    {
        public int value;
    }

    public class DummyCommand : ICommand
    {
        public int Payload;
    }

    public static class DummyReducers
    {
        internal static void RegisterAll(FluxityFeatureContext<DummyState> context)
        {
            context.Reducer<DummyCommand>(Reduce);
        }

        public static DummyState Reduce(DummyState state, DummyCommand command)
        {
            return new DummyState() { value = state.value + command.Payload };
        }
    }
}
