﻿using AIR.Flume;

namespace AIR.Fluxity
{
    public static class FlumeServiceContainerExt
    {
        public static FlumeServiceContainer RegisterFluxity(this FlumeServiceContainer self)
            => self
                .Register<IStore, Store>()
                .Register<IDispatcher, Dispatcher>();

        public static FlumeServiceContainer RegisterFeature<TState>(this FlumeServiceContainer self)
            where TState : struct
            => self
                .Register<IFeature<TState>, Feature<TState>>();
    }
}