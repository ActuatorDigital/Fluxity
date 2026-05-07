using System;
using System.Collections.Generic;

namespace AIR.Fluxity
{
    public interface IFeature<TState> : IFeatureObservable<TState>, IFeature
        where TState : struct
    {
        void SetState(TState newState);
    }

    public interface IFeatureObservable<TState> : IFeatureView<TState>
        where TState : struct
    {
        event Action<TState> OnStateChanged;
    }

    public interface IFeatureView<TState>
        where TState : struct
    {
        TState State { get; }
    }

    public interface IFeature : IFeatureReduce
    {
        Type GetStateType { get; }

        void ProcessReducers(ICommand command);
        IReadOnlyCollection<Type> GetAllHandledCommandTypes();
        IReadOnlyCollection<IReducer> GetAllReducersForCommand(Type commandType);
    }

    public interface IFeatureReduce
    {
        void Register(IReducer reducer);
    }
}