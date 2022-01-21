using AIR.Flume;
using AIR.Fluxity;

public class ProjectServiceInstaller : ServiceInstaller
{
    protected override void InstallServices(FlumeServiceContainer container)
    {
        container
            .RegisterFluxity()
            .RegisterFeature<CounterState>()
            ;
    }
}
