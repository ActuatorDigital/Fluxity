using System;
using Actuator.Fluxity;

namespace Examples.ObjectData
{
    public class AddObjectDataCommand : ICommand
    {
        public Guid[] GuidsToAdd;
    }
}