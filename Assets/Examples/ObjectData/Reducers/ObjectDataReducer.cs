using System.Linq;

namespace Examples.ObjectData
{
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