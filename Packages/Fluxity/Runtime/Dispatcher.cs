using System;
using System.Collections.Generic;

namespace AIR.Fluxity
{
    public sealed class Dispatcher : IDispatcher
    {
        public event Action<ICommand> OnDispatch;

        private const int MaxDequeueDispatches = 1000;
        private readonly Dictionary<Type, List<IEffect>> _effects = new();
        private readonly Queue<ICommand> _commandQueue = new();
        private bool _isDequeing;
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
            _commandQueue.Enqueue(command);
            if (_isDequeing) return;

            InnerDispatch(command);
        }

        private void InnerDispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            _isDequeing = true;
            var count = 0;
            while (_commandQueue.Count > 0)
            {
                var nextCommand = _commandQueue.Dequeue();
                ProcessCommand(nextCommand);
                if (count++ > MaxDequeueDispatches)
                {
                    throw new DispatcherException($"Dispatcher is stuck in a loop. Check for circular dependencies in your effects. " +
                        $"Current command '{nextCommand.GetType()}', exceeded '{MaxDequeueDispatches}' deque dispatches.");
                }
            }
            _isDequeing = false;
        }

        private void ProcessCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            OnDispatch?.Invoke(command);
            _store.ProcessCommand(command);
            ProcessEffects(command);
        }

        public IReadOnlyCollection<Type> GetAllEffectCommandTypes()
            => _effects.Keys;

        public IReadOnlyCollection<IEffect> GetAllEffectsForCommandType(Type commandType)
            => _effects[commandType].AsReadOnly();

        private void ProcessEffects<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (!_effects.TryGetValue(command.GetType(), out var extantEffects))
                return;

            foreach (var effect in extantEffects)
                effect.DoEffect(command, this);
        }
    }

    [Serializable]
    public class DispatcherException : Exception
    {
        public DispatcherException(string message)
            : base(message)
        {
        }
    }
}