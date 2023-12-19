using System;
using AIR.Flume;

namespace AIR.Fluxity
{
    public class FluxityFlumeRegisterContext
    {
        public FluxityFlumeRegisterContext(Store store, Dispatcher dispatcher, FlumeServiceContainer flumeServiceContainer)
        {
            Store = store;
            Dispatcher = dispatcher;
            FlumeServiceContainer = flumeServiceContainer;
        }

        public Store Store { get; }
        public Dispatcher Dispatcher { get; }
        public FlumeServiceContainer FlumeServiceContainer { get; }

        public FluxityFeatureContext<TState> Feature<TState>(TState startingValue)
            where TState : struct
        {
            var feat = new Feature<TState>(startingValue);
            Store.AddFeature(feat);
            return new FluxityFeatureContext<TState>(this, feat);
        }

        public FluxityFeatureContext<TState> Feature<TState>(
            TState startingValue,
            Action<FluxityFeatureContext<TState>> createReducers)
            where TState : struct
        {
            var context = Feature(startingValue);
            createReducers(context);
            return context;
        }
    }
}