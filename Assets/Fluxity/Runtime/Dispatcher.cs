using System;
using System.Collections.Generic;
using AIR.Flume;

namespace AIR.Fluxity
{
    public class Dispatcher : Dependent, IDispatcher
    {
        private readonly Dictionary<Type, List<IEffect>> _effects = new Dictionary<Type, List<IEffect>>();
        private bool _isReducing;
        private IStore _store;

        public void Inject(IStore store)
            => _store = store;

        public void Register<TCommand>(IEffect<TCommand> effect)
            where TCommand : ICommand
        {
            if (_effects.TryGetValue(typeof(TCommand), out var extantEffects))
            {
                extantEffects.Add(effect);
                return;
            }

            _effects.Add(typeof(TCommand), new List<IEffect>() { effect });
        }

        public void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (_isReducing)
                throw new DispatcherException($"Attempted to dispatch '{command.GetType()}' during an existing {nameof(Dispatch)}.");

            _isReducing = true;
            _store.ProcessCommand(command);
            _isReducing = false;

            ProcessEffects(command);
        }

        private void ProcessEffects<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (_effects.TryGetValue(typeof(TCommand), out var extantEffects))
            {
                foreach (var effect in extantEffects)
                {
                    effect.DoEffect(command);
                }
            }
        }
    }
}