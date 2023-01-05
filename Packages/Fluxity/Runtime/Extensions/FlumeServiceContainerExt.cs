using AIR.Flume;

namespace AIR.Fluxity
{
    public static class FlumeServiceContainerExt
    {
        public static FlumeServiceContainer RegisterFluxity(this FlumeServiceContainer self)
            => self
                .Register<IStore, Store>()
                .Register<IDispatcher, Dispatcher>();

        public static FlumeServiceContainer RegisterFeature<TState>(this FlumeServiceContainer self, TState startingValue)
            where TState : struct
            => self
                .Register<IFeature<TState>>(new Feature<TState>(startingValue));

        public static FlumeServiceContainer RegisterFeature<TState>(this FlumeServiceContainer self)
            where TState : struct
            => RegisterFeature(self, default(TState));
    }
}