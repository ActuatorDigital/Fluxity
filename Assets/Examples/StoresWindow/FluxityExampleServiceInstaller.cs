using AIR.Flume;
using AIR.Fluxity;
using UnityEngine;

namespace Examples.StoresWindow
{
    [DefaultExecutionOrder(-1)]
    public class FluxityExampleServiceInstaller : ServiceInstaller
    {
        protected override void InstallServices(FlumeServiceContainer container)
        {
            container
                .RegisterFluxity()
                .RegisterFeature(ListOfIntsState.CreateDefault())
                .RegisterFeature(SimpleState.CreateDefault())
                .RegisterFeature(TransformsState.CreateDefault())
                .RegisterFeature(CustomObjectCyclicState.CreateDefault())
                .RegisterFeature(CommonTypesState.CreateDefault())
                .RegisterFeature(DicOfStringTransformState.CreateDefault())
            ;
        }
    }
}