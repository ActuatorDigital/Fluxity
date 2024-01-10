using AIR.Flume;
using AIR.Fluxity;

namespace Examples.Simple
{
    public class FluxityExampleInitializer : FluxityInitializer
    {
        public override void Register(FluxityRegisterContext context)
        {
            context
                .Feature(new CounterState(), CounterReducer.RegisterAll)
                .Effect<IncrementCountCommand>(CounterEffects.DoIncrementEffect)
                .Effect<DecrementCountCommand>(CounterEffects.DoDecrementEffect)
                ;
        }

        protected override void Install(FlumeServiceContainer container)
        {
        }
    }
}