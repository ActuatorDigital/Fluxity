using System;
using System.Collections.Generic;

namespace AIR.Fluxity
{
    public interface IFeature<TState> : IFeature
        where TState : struct
    {
        event Action<TState> OnStateChanged;

        TState State { get; }

        void SetState(TState newState);
    }

    public interface IFeature
    {
        Type GetStateType { get; }

        void Register(IReducer reducer);

        void ProcessReducers(ICommand command);
        IReadOnlyCollection<Type> GetAllHandledCommandTypes();
        IReadOnlyCollection<IReducer> GetAllReducersForCommand(Type commandType);
    }
}