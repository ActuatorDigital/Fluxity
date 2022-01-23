namespace AIR.Fluxity.Tests.DummyTypes
{
    public static class DummyPureFunctionReducer
    {
        public static DummyState Reduce(DummyState state, DummyCommand command)
        {
            return new DummyState() { value = state.value + command.payload };
        }
    }
}
