using AIR.Flume;

namespace AIR.Fluxity
{
    public abstract class FluxityInitializer : DependentBehaviour
    {
        private IStore _store;
        private IDispatcher _dispatcher;

        public void Inject(
            IDispatcher dispatcher,
            IStore store)
        {
            _store = store;
            _dispatcher = dispatcher;

            Initialize();
            PostInitialize(_dispatcher);
        }

        public void CreateReducer<TState, TCommand>(IReducer<TState, TCommand>.ReduceDelegate pureFunctionReducer)
            where TState : struct
            where TCommand : ICommand
            => _store.CreateAndRegister(pureFunctionReducer);

        public void CreateEffect<TCommand>(IEffect<TCommand>.EffectDelegate effectAction)
               where TCommand : ICommand
        {
            var effect = new EffectBinding<TCommand>(effectAction);
            _dispatcher.RegisterEffect(effect);
        }

        protected abstract void Initialize();

        protected virtual void PostInitialize(IDispatcher dispatcher) { }
    }
}