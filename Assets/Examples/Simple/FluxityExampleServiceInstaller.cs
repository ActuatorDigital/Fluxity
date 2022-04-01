using AIR.Flume;
using AIR.Fluxity;

namespace Examples.Counter
{
    public class FluxityExampleServiceInstaller : ServiceInstaller
    {
        protected override void InstallServices(FlumeServiceContainer container)
        {
            container
                .RegisterFluxity()
                .RegisterFeature<CounterState>()
                ;
        }
    }
}