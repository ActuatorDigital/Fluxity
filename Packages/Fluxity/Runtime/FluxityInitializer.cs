﻿using AIR.Flume;

namespace AIR.Fluxity
{
    public abstract class FluxityInitializer : DependentBehaviour
    {
        private IDispatcher _dispatcher;

        public void Inject(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            CreateEffects();
            PostInitialize(_dispatcher);
        }

        public void CreateEffect<TCommand>(IEffect<TCommand>.EffectDelegate effectAction)
               where TCommand : ICommand
        {
            var effect = new EffectBinding<TCommand>(effectAction);
            _dispatcher.RegisterEffect(effect);
        }

        protected abstract void CreateEffects();

        protected virtual void PostInitialize(IDispatcher dispatcher) { }
    }
}