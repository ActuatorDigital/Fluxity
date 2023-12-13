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
}