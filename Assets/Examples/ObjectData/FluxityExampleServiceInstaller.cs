using AIR.Flume;
using AIR.Fluxity;
using UnityEngine;

namespace Examples.ObjectData
{
    [DefaultExecutionOrder(-1)]
    public class FluxityExampleServiceInstaller : ServiceInstaller
    {
        protected override void InstallServices(FlumeServiceContainer container)
        {
            container
                .RegisterFluxity(x =>
                    x.Feature(ObjectDataState.Create())
                        .Reducer<AddObjectDataCommand>(ObjectDataReducer.AddData))
                ;
        }
    }
}