using System;
using AIR.Fluxity;

namespace Examples.ObjectData
{
    public class AddObjectDataCommand : ICommand
    {
        public Guid[] GuidsToAdd;
    }
}