using System.Collections.Generic;

namespace AIR.Fluxity
{
    public interface IStore
    {
        IReadOnlyCollection<IFeature> GetAllFeatures();
        void ProcessCommand<TCommand>(TCommand command)
            where TCommand : ICommand;

        void AddFeature<TState>(IFeature<TState> feature)
            where TState : struct;

        void RegisterReducer<TState, TCommand>(IReducer<TState, TCommand> reducer)
            where TState : struct
            where TCommand : ICommand;

        IFeatureView<TState> GetFeatureView<TState>()
            where TState : struct;
    }
}