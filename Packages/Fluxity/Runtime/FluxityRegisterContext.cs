using System;

namespace AIR.Fluxity
{
    public class FluxityRegisterContext
    {
        protected readonly IStore _store;
        protected readonly IDispatcher _dispatcher;

        public FluxityRegisterContext(
            IStore store,
            IDispatcher dispatcher)
        {
            _store = store;
            _dispatcher = dispatcher;
        }

        public FluxityRegisterContext OnPreDispatch<TCommand>(Action<TCommand> del) where TCommand : ICommand
        {
            _store.AddPreAction(del);
            return this;
        }

        public FluxityRegisterContext OnPostDispatch<TCommand>(Action<TCommand> del) where TCommand : ICommand
        {
            _store.AddPostAction(del);
            return this;
        }

        public FluxityRegisterFeatureContext<TState> Feature<TState>(TState startingValue)
            where TState : struct
        {
            var feat = new Feature<TState>(startingValue);
            _store.AddFeature(feat);
            return new FluxityRegisterFeatureContext<TState>(_store, _dispatcher, feat);
        }

        public FluxityRegisterFeatureContext<TState> Feature<TState>(
            TState startingValue,
            Action<FluxityRegisterFeatureContext<TState>> createReducers)
            where TState : struct
        {
            var context = Feature(startingValue);
            createReducers(context);
            return context;
        }
    }
}