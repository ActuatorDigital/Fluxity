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

        public FluxityFeatureContext<TState> Feature<TState>(TState startingValue)
            where TState : struct
        {
            var feat = new Feature<TState>(startingValue);
            _store.AddFeature(feat);
            return new FluxityFeatureContext<TState>(_store, _dispatcher, feat);
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

        public FluxityRegisterContext Effect<TCommand>(IEffect<TCommand>.EffectDelegate effectAction)
               where TCommand : ICommand
        {
            var effect = new EffectBinding<TCommand>(effectAction);
            _dispatcher.RegisterEffect(effect);
            return this;
        }

        public FluxityEffectContext<T> Effect<T>(T instanceContext)
            where T : class
        {
            return new FluxityEffectContext<T>(_store, _dispatcher, instanceContext);
        }

        public FluxityRegisterContext Effect<T>(
            T instanceContext,
            Action<FluxityEffectContext<T>> createEffects)
            where T : class
        {
            var res = new FluxityEffectContext<T>(_store, _dispatcher, instanceContext);
            createEffects(res);
            return this;
        }
    }
}