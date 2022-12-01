namespace AIR.Fluxity.Tests.DummyTypes
{
    internal class DummyEffect : Effect<DummyCommand>
    {
        public int accumPayload;

        public DummyEffect(IDispatcher dispatcher)
            : base(dispatcher)
        {
        }

        public override void DoEffect(DummyCommand command)
        {
            accumPayload += command.payload;
        }
    }
}