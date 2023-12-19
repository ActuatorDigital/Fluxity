using System.Collections.Generic;
using System.Linq;

namespace Examples.ObjectData
{
    // C# in Unity does not allow paramless struct ctor or initial values, so
    //  this will be null if default. So when this is RegisterFeature, we give it 
    //  a starting value.
    public struct ObjectDataState
    {
        public static ObjectDataState Create() => new() { Guids = new List<System.Guid>() };

        public List<System.Guid> Guids;
    }

    public static class ObjectDataReducer
    {
        public static ObjectDataState AddData(ObjectDataState state, AddObjectDataCommand command)
        {
            var guids = state.Guids;
            guids.AddRange(command.GuidsToAdd);
            return new ObjectDataState { Guids = guids.Distinct().ToList() };
        }
    }
}