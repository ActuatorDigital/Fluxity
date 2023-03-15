using System;

namespace AIR.Fluxity
{
    public abstract class Reducer<TState, TCommand> : IReducer<TState, TCommand>
        where TState : struct
        where TCommand : ICommand
    {
        public Type CommandType => typeof(TCommand);

        public abstract TState Reduce(TState state, TCommand command);

        public TState Reduce(TState state, ICommand command)
            => Reduce(state, (TCommand)command);
    }
}