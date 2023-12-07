using AIR.Flume;
using AIR.Fluxity;
using Examples.Countdown;
using UnityEngine;

namespace Examples.Countdown
{
    [DefaultExecutionOrder(-1)]
    public class FluxityExampleServiceInstaller : ServiceInstaller
    {
        protected override void InstallServices(FlumeServiceContainer container)
        {
            container
                .RegisterFluxity(x => 
                    x.Feature(new CountdownState())
                        .Reducer<StartCountdownCommand>(CountdownReducer.StartCountDown)
                        .Reducer<StopCountdownCommand>(CountdownReducer.StopCountDown))
                ;
        }
    }
}