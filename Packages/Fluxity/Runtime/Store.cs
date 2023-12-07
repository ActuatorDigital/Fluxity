using System;
using System.Collections.Generic;

namespace AIR.Fluxity
{
    public class Store : IStore
    {
        private readonly Dictionary<Type, IFeature> _features = new();

        public IReadOnlyCollection<IFeature> GetAllFeatures()
        {
            return _features.Values;
        }

        public void AddFeature<TState>(IFeature<TState> feature)
            where TState : struct
        {
            _features[feature.GetStateType] = feature;
        }

        public void RegisterReducer<TState, TCommand>(IReducer<TState, TCommand> reducer)
            where TState : struct
            where TCommand : ICommand
        {
            _features[typeof(TState)].Register(reducer);
        }

        public void ProcessCommand<TCommand>(TCommand command)
          where TCommand : ICommand
        {
            foreach (var feature in _features.Values)
            {
                feature.ProcessReducers(command);
            }
        }

        public IFeatureView<TState> GetFeatureView<TState>()
            where TState : struct
        {
            return (IFeatureView<TState>)_features[typeof(TState)];
        }
    }
}