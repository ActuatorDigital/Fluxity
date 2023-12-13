using AIR.Flume;
using AIR.Fluxity;
using Examples.Countdown;
using UnityEngine;

namespace Examples.GameSession
{
    [DefaultExecutionOrder(-1)]
    public class FluxityExampleServiceInstaller : ServiceInstaller
    {
        protected override void InstallServices(FlumeServiceContainer container)
        {
            container
                .RegisterFluxity(FluxityExampleInitializer.Setup)
                ;
        }
    }
}