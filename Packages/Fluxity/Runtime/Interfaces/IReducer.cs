using System;
using System.Reflection;

namespace AIR.Fluxity
{
    public interface IReducer<TState, TCommand> : IReducer<TState>
        where TState : struct
        where TCommand : ICommand
    {
        public delegate TState ReduceDelegate(TState state, TCommand command);

        TState Reduce(TState state, TCommand command);
    }

    public interface IReducer<TState> : IReducer
        where TState : struct
    {
        TState Reduce(TState state, ICommand command);
    }

    public interface IReducer
    {
        Type CommandType { get; }
        MethodInfo ReducerBindingInfo();
    }
}