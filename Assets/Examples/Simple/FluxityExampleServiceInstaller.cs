using AIR.Flume;
using AIR.Fluxity;
using UnityEngine;

namespace Examples.Simple
{
    [DefaultExecutionOrder(-1)]
    public class FluxityExampleServiceInstaller : ServiceInstaller
    {
        protected override void InstallServices(FlumeServiceContainer container)
        {
            container
                .RegisterFluxity(x => 
                    x.Feature(new CounterState())
                        .Reducer<IncrementCountCommand>(CounterReducer.Increment)
                        .Reducer<DecrementCountCommand>(CounterReducer.Decrement))
                ;
        }
    }
}