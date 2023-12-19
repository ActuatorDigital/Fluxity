using System;
using System.Collections.Generic;

namespace AIR.Fluxity
{
    public sealed class Dispatcher : IDispatcher
    {
        public event Action<ICommand> OnDispatch;

        private readonly Dictionary<Type, List<IEffect>> _effects = new();
        private bool _isReducing;
        private readonly IStore _store;

        public Dispatcher(IStore store)
        {
            _store = store;
        }

        public void RegisterEffect<TCommand>(IEffect<TCommand> effect)
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
            OnDispatch?.Invoke(command);
            if (_isReducing)
                throw new DispatcherException($"Attempted to dispatch '{command.GetType()}' during an existing {nameof(Dispatch)}.");

            _isReducing = true;
            _store.ProcessCommand(command);
            _isReducing = false;

            ProcessEffects(command);
        }

        public IReadOnlyCollection<Type> GetAllEffectCommandTypes()
            => _effects.Keys;

        public IReadOnlyCollection<IEffect> GetAllEffectsForCommandType(Type commandType)
            => _effects[commandType].AsReadOnly();

        private void ProcessEffects<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (!_effects.TryGetValue(typeof(TCommand), out var extantEffects))
                return;

            foreach (var effect in extantEffects)
                effect.DoEffect(command, this);
        }
    }
}