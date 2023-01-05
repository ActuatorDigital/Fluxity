using AIR.Fluxity;
using UnityEngine;

namespace Examples.ObjectData
{
    [DefaultExecutionOrder(1)]
    public class FluxityExampleInitializer : FluxityInitializer
    {
        protected override void Initialize()
        {
            CreateReducer<ObjectDataState, AddObjectDataCommand>(ObjectDataReducer.AddData);
        }
    }
}