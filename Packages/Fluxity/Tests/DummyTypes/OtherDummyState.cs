namespace AIR.Fluxity.Tests.DummyTypes
{
    public struct OtherDummyState { }

    internal class OtherDummyCommand : ICommand { }
    internal class OtherOtherDummyCommand : ICommand { }

    internal static class OtherDummyReducer
    {
        public static OtherDummyState Reduce(OtherDummyState state, OtherDummyCommand command)
        {
            return new OtherDummyState();
        }
    }
}
