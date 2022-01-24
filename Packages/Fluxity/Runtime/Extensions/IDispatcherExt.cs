using System;

namespace AIR.Fluxity
{
    public static class IDispatcherExt
    {
        public static TEffect CreateAndRegister<TEffect, TCommand>(this IDispatcher dispatcher)
            where TEffect : Effect<TCommand>
            where TCommand : ICommand
        {
            var effect = (TEffect)Activator.CreateInstance(typeof(TEffect), dispatcher);
            dispatcher.RegisterEffect(effect);
            return effect;
        }
    }
}