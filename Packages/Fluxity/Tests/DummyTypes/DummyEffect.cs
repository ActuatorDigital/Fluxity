namespace AIR.Fluxity.Tests.DummyTypes
{
    internal class DummyEffect
    {
        public int accumPayload;

        public void DoEffect(DummyCommand command, IDispatcher dispatcher)
        {
            accumPayload += command.Payload;
        }
    }
}