using System;
using AIR.Flume;

namespace AIR.Fluxity
{
    public static class FlumeServiceContainerExt
    {
        public static FlumeServiceContainer RegisterFluxity(this FlumeServiceContainer self, Action<FluxityFlumeRegisterContext> action)
        {
            var store = new Store();
            var distpatcher = new Dispatcher(store);
            self.Register<IStore>(store);
            self.Register<IDispatcher>(distpatcher);
            action(new FluxityFlumeRegisterContext(store, distpatcher, self));
            return self;
        }
    }

    public class FluxityFlumeRegisterContext
    {
        public FluxityFlumeRegisterContext(Store store, Dispatcher distpatcher, FlumeServiceContainer flumeServiceContainer)
        {
            Store = store;
            Distpatcher = distpatcher;
            FlumeServiceContainer = flumeServiceContainer;
        }

        public Store Store { get; }
        public Dispatcher Distpatcher { get; }
        public FlumeServiceContainer FlumeServiceContainer { get; }

        public FluxityFeatureContext<TState> Feature<TState>(TState startingValue)
            where TState : struct
        {
            var feat = new Feature<TState>(startingValue);
            Store.AddFeature(feat);
            return new FluxityFeatureContext<TState>(this, feat);
        }
    }

    public class FluxityFeatureContext<TState> : FluxityFlumeRegisterContext
        where TState : struct
    {
        private readonly Feature<TState> _feature;
        public FluxityFeatureContext(FluxityFlumeRegisterContext context, Feature<TState> feature)
            : base(context.Store, context.Distpatcher, context.FlumeServiceContainer)
        {
            _feature = feature;
        }

        public FluxityFeatureContext<TState> Reducer<TCommand>(IReducer<TState, TCommand>.ReduceDelegate pureFunctionReducer)
            where TCommand : ICommand
        {
            var reducer = new PureFunctionReducerBinder<TState, TCommand>(pureFunctionReducer);
            _feature.Register(reducer);
            return this;
        }
    }
}