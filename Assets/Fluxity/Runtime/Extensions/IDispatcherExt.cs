using System;

namespace AIR.Fluxity
{
    public static class IDispatcherExt
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