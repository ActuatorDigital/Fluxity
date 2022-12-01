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

        public IReducer CreateReducer<TState, TCommand>(IReducer<TState, TCommand>.ReduceDelegate pureFunctionReducer)
            where TState : struct
            where TCommand : ICommand
            => _store.CreateAndRegister(pureFunctionReducer);

        public TEffect CreateEffect<TEffect, TCommand>()
               where TEffect : Effect<TCommand>
               where TCommand : ICommand
            => _dispatcher.CreateAndRegister<TEffect, TCommand>();

        protected abstract void Initialize();

        protected virtual void PostInitialize(IDispatcher dispatcher) { }
    }
}