using System;
using System.Collections.Generic;

namespace AIR.Fluxity
{
    public sealed class Dispatcher : IDispatcher
    {
        public event Action<ICommand> OnDispatch;

        private const int MaxDequeueDispatches = 1000;
        private readonly Queue<ICommand> _commandQueue = new();
        private bool _isDequeuing;
        private readonly IStore _store;

        public Dispatcher(IStore store)
        {
            _store = store;
        }
        public void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            _commandQueue.Enqueue(command);
            if (_isDequeuing) return;

            InnerDispatch();
        }

        private void InnerDispatch() 
        {
            _isDequeuing = true;
            var count = 0;
            while (_commandQueue.Count > 0)
            {
                var cmd = _commandQueue.Dequeue();

                OnDispatch?.Invoke(cmd);
                _store.ProcessCommand(cmd);

                if (count++ > MaxDequeueDispatches)
                {
                    throw new DispatcherException($"Dispatcher has reached the {nameof(MaxDequeueDispatches)} ({MaxDequeueDispatches}). " +
                        $"Check for circular dependencies in your effects. " +
                        $"Current command '{cmd.GetType()}'.");
                }
            }
            _isDequeuing = false;
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