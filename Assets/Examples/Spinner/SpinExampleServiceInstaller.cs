using AIR.Flume;
using AIR.Fluxity;
using UnityEngine;

namespace Examples.Spinner
{
    [DefaultExecutionOrder(-1)]
    public class SpinExampleServiceInstaller : ServiceInstaller
    {
        protected override void InstallServices(FlumeServiceContainer container)
        {
            container
                .RegisterFluxity(x =>
                    x.Feature(new SpinState())
                        .Reducer<StartSpinCommand>(SpinnerReducers.StartSpin)
                        .Reducer<StopSpinCommand>(SpinnerReducers.StopSpin))
                ;
        }
    }
}