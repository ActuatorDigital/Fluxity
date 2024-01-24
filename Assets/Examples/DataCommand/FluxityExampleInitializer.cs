using AIR.Flume;
using AIR.Fluxity;

namespace Examples.DataCommand
{
    public class FluxityExampleInitializer : FluxityInitializer
    {
        public override void RegisterFluxity(FluxityRegisterContext context)
        {
            context
                .Feature(new CounterState())
                    .Reducer<ChangeCountCommand>(CounterReducer.Change)
                .Effect(new ChangeCounterEffect())
                    .Method<ChangeCountCommand>(x => x.DoEffect)
                ;
        }

        protected override void RegisterServices(FlumeServiceContainer container)
        {
            container
                .Register<ISomeService, SomeService>()
                ;
        }
    }
}