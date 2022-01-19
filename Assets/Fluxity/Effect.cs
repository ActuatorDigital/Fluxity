using AIR.Flume;
using System;

namespace AIR.Fluxity
{
    public abstract class Effect<TCommand> : Dependent, IEffect<TCommand>
        where TCommand : ICommand
    {
        protected IDispatcher Dispatcher { get; private set; }

        protected Effect(IDispatcher dispatcher) 
            => Dispatcher = dispatcher;

        public abstract void DoEffect(TCommand command);

        public void DoEffect(ICommand command) 
            => DoEffect((TCommand)command);
    }

    public static class DispatcherEffectExt
    {
        public static TEffect CreateAndRegisterEffect<TEffect, TCommand>(this IDispatcher dispatcher)
            where TEffect : Effect<TCommand>
            where TCommand : ICommand
        {
            var effect = (TEffect)Activator.CreateInstance(typeof(TEffect), dispatcher);
            dispatcher.Register(effect);
            return effect;
        }
    }
}