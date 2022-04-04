using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AIR.Flume;

namespace AIR.Fluxity
{
    public class Dispatcher : Dependent, IDispatcher, IDisposable
    {
        private readonly Dictionary<Type, List<IEffect>> _effects = new Dictionary<Type, List<IEffect>>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private bool _isReducing;
        private IStore _store;

        public void Inject(IStore store)
            => _store = store;

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
            if (_cancellationTokenSource.IsCancellationRequested)
                return;

            if (_isReducing)
                throw new DispatcherException($"Attempted to dispatch '{command.GetType()}' during an existing {nameof(Dispatch)}.");

            _isReducing = true;
            _store.ProcessCommand(command);
            _isReducing = false;

            ProcessEffects(command);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        private void ProcessEffects<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            var effectTasks = new List<Task>();
            var exceptions = new List<Exception>();
            effectTasks.Capacity = _effects.Count;

            if (_effects.TryGetValue(typeof(TCommand), out var extantEffects))
            {
                foreach (var effect in extantEffects)
                {
                    try
                    {
                        var t = effect.DoEffect(command);
                        if (t.IsCompleted)
                            continue;

                        effectTasks.Add(Task.Run(async () => await t, _cancellationTokenSource.Token));
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                }
            }

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);

            exceptions.Clear();
            if (effectTasks.Count > 0)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await Task.WhenAll(effectTasks);
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }

                    if (exceptions.Count > 0)
                        throw new AggregateException(exceptions);
                });
            }
        }
    }
} 