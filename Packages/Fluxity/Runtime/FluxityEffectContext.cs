using System;

namespace AIR.Fluxity
{
    public sealed class FluxityEffectContext<T> : FluxityRegisterContext
    {
        private readonly T _instance;

        public FluxityEffectContext(
            IStore store,
            IDispatcher dispatcher,
            T instanceContext)
            : base(store, dispatcher)
        {
            _instance = instanceContext;
        }

        public FluxityEffectContext<T> Method<TCommand>(Func<T, IEffect<TCommand>.EffectDelegate> make)
            where TCommand : ICommand
        {
            var effect = new EffectBinding<TCommand>(make(_instance));
            _dispatcher.RegisterEffect(effect);
            return this;
        }
    }
}