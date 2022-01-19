﻿using System;
using System.Collections.Generic;

namespace AIR.Fluxity
{
    public class Store : IStore
    {
        private readonly Dictionary<Type, IFeature> _features = new Dictionary<Type, IFeature>();

        public void AddFeature<TState>(IFeature<TState> feature)
            where TState : struct
        {
            _features[feature.GetStateType] = feature;
        }

        public void Register<TState, TCommand>(IReducer<TState, TCommand> reducer)
            where TState : struct
            where TCommand : ICommand
        {
            _features[reducer.GetStateType].Register(reducer);
        }

        public void ProcessCommand<TCommand>(TCommand command)
          where TCommand : ICommand
        {
            foreach (var feature in _features.Values)
            {
                feature.ProcessReducers(command);
            }
        }
    }

    public static class StorePureFunctionReducerExt
    {
        public static IReducer CreateAndRegister<TState, TCommand>(
            this IStore self,
            IReducer.Reduce<TState,TCommand> pureFunctionReducer)
            where TState : struct
            where TCommand : ICommand
        {
            var reducer = new PureFunctionReducerBinder<TState, TCommand>(pureFunctionReducer);
            self.Register(reducer);
            return reducer;
        }
    }
}