using AIR.Flume;
using AIR.Fluxity;
using UnityEngine;

namespace Examples.DataCommand
{
    [DefaultExecutionOrder(-1)]
    public class FluxityExampleServiceInstaller : ServiceInstaller
    {
        protected override void InstallServices(FlumeServiceContainer container)
        {
            container
                .RegisterFluxity(x => x.RegisterFeature(new CounterState()))

                .Register<ISomeService, SomeService>()
                ;
        }
    }
}