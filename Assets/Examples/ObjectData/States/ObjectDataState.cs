using System.Collections.Generic;

namespace Examples.ObjectData
{
    // C# in Unity does not allow paramless struct ctor or initial values, so
    //  this will be null if default. So when this is RegisterFeature, we give it 
    //  a starting value.
    public struct ObjectDataState
    {
        public static ObjectDataState CreateDefault() => new ObjectDataState { Guids = new List<System.Guid>() };

        public List<System.Guid> Guids;
    }
}