using AIR.Flume;
using AIR.Fluxity;
using UnityEngine;

namespace Examples.GameSession
{
    [DefaultExecutionOrder(-1)]
    public class FluxityExampleServiceInstaller : ServiceInstaller
    {
        [SerializeField] private FluxityExampleInitializer _fluxityExampleInitializer;

        protected override void InstallServices(FlumeServiceContainer container)
        {
            container
                .RegisterFluxity(_fluxityExampleInitializer.Setup)
                ;
        }
    }
}