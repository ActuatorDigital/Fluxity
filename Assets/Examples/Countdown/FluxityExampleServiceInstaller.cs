using AIR.Flume;
using AIR.Fluxity;
using UnityEngine;

namespace Examples.Countdown
{
    [DefaultExecutionOrder(-1)]
    public class FluxityExampleServiceInstaller : ServiceInstaller
    {
        protected override void InstallServices(FlumeServiceContainer container)
        {
            container
                .RegisterFluxity()
                .RegisterFeature<CountdownState>()
                ;
        }
    }
}