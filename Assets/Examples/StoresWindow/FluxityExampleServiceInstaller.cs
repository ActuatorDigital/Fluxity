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
                .RegisterFluxity(x =>
                {
                    x.RegisterFeature(ListOfIntsState.CreateDefault());
                    x.RegisterFeature(SimpleState.CreateDefault());
                    x.RegisterFeature(TransformsState.CreateDefault());
                    x.RegisterFeature(CustomObjectCyclicState.CreateDefault());
                    x.RegisterFeature(CommonTypesState.CreateDefault());
                    x.RegisterFeature(DicOfStringTransformState.CreateDefault());
                    x.RegisterFeature(SomeDatumStateWithEnum.CreateDefault());
                    x.RegisterFeature(SomeNullDatumState.CreateDefault());
                })
            ;
        }
    }
}