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
                    x.Feature(ListOfIntsState.CreateDefault());
                    x.Feature(SimpleState.CreateDefault());
                    x.Feature(TransformsState.CreateDefault());
                    x.Feature(CustomObjectCyclicState.CreateDefault());
                    x.Feature(CommonTypesState.CreateDefault());
                    x.Feature(DicOfStringTransformState.CreateDefault());
                    x.Feature(SomeDatumStateWithEnum.CreateDefault());
                    x.Feature(SomeNullDatumState.CreateDefault());
                })
            ;
        }
    }
}