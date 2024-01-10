using AIR.Flume;
using AIR.Fluxity;
using UnityEngine;

namespace Examples.StoresWindow
{
    [DefaultExecutionOrder(-1)]
    public class FluxityExampleInitializer : FluxityInitializer
    {
        public override void Register(FluxityRegisterContext context)
        {
            context
                .Feature(ListOfIntsState.CreateDefault())
                .Feature(SimpleState.CreateDefault())
                .Feature(TransformsState.CreateDefault())
                .Feature(CustomObjectCyclicState.CreateDefault())
                .Feature(CommonTypesState.CreateDefault())
                .Feature(DicOfStringTransformState.CreateDefault())
                .Feature(SomeDatumStateWithEnum.CreateDefault())
                .Feature(SomeNullDatumState.CreateDefault())
                ;
        }

        protected override void Install(FlumeServiceContainer container)
        {
        }
    }
}