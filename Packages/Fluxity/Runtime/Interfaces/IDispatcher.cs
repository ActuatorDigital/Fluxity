using System;
using System.Collections.Generic;

namespace AIR.Fluxity
{
    public interface IDispatcher
    {
        event Action<ICommand> OnDispatch;

        public void RegisterEffect<TCommand>(IEffect<TCommand> effect)
            where TCommand : ICommand;

        public void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand;
        
        IReadOnlyCollection<Type> GetAllEffectCommandTypes();
        IReadOnlyCollection<IEffect> GetAllEffectsForCommandType(Type commandType);
    }
}