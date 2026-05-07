using AIR.Flume;
using AIR.Fluxity;

namespace Examples.Simple
{
    public class FluxityExampleInitializer : FluxityInitializer
    {
        public override void RegisterFluxity(FluxityRegisterContext context)
        {
            context
                .Feature(new CounterState(), CounterReducer.RegisterAll)
                ;
        }

        protected override void RegisterServices(FlumeServiceContainer container)
        {
        }
    }
}