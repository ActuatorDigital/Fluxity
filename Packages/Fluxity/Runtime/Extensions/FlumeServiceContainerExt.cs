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

        public FluxityFlumeRegisterContext RegisterFeature<TState>(TState startingValue)
            where TState : struct
        {
            Store.AddFeature(new Feature<TState>(startingValue));
            return this;
        }
    }
}