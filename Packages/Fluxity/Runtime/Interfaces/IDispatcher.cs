using System;

namespace AIR.Fluxity
{
    public interface IDispatcher
    {
        event Action<ICommand> OnDispatch;

        public void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}