using AIR.Flume;
using UnityEngine;

namespace AIR.Fluxity
{
    [DefaultExecutionOrder(-1)]
    [RequireComponent(typeof(FlumeServiceContainer))]
    public abstract class FluxityInitializer : MonoBehaviour
    {
        private Dispatcher _dispatcher;

        public virtual void Awake()
        {
            var container = gameObject.GetComponent<FlumeServiceContainer>();
            container.OnContainerReady += InstallServices;

            var store = new Store();
            _dispatcher = new Dispatcher(store);
            var registerContext = new FluxityRegisterContext(store, _dispatcher);
            container.Register<IStore>(store);
            container.Register<IDispatcher>(_dispatcher);
            RegisterFluxity(registerContext);
        }

        private void InstallServices(FlumeServiceContainer container)
        {
            RegisterServices(container);
            PostInitialize(_dispatcher);
        }

        protected abstract void RegisterServices(FlumeServiceContainer container);

        public abstract void RegisterFluxity(FluxityRegisterContext context);

        protected virtual void PostInitialize(IDispatcher dispatcher) { }
    }
}