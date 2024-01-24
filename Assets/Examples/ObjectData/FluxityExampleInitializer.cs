using AIR.Flume;
using AIR.Fluxity;
using UnityEngine;

namespace Examples.ObjectData
{
    [DefaultExecutionOrder(-1)]
    public class FluxityExampleInitializer : FluxityInitializer
    {
        public override void RegisterFluxity(FluxityRegisterContext context)
        {
            context
                .Feature(ObjectDataState.Create())
                        .Reducer<AddObjectDataCommand>(ObjectDataReducer.AddData)
                ;
        }

        protected override void RegisterServices(FlumeServiceContainer container)
        {
        }
    }
}