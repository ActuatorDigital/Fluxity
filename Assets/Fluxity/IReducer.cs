using System;

namespace AIR.Fluxity
{
    public interface IReducer<TState, TCommand> : IReducer<TState>
        where TState : struct
        where TCommand : ICommand
    {
        TState Reduce(TState state, TCommand command);
    }

    public interface IReducer<TState> : IReducer
        where TState : struct
    {
        TState Reduce(TState state, ICommand command);
    }

    public interface IReducer
    {
        public delegate TState Reduce<TState, TCommand>(TState state, TCommand command)
            where TState : struct
            where TCommand : ICommand;

        Type GetCommandType { get; }
        Type GetStateType { get; }
    }
}