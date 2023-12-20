using System;
using System.Collections.Generic;
using AIR.Flume;

namespace AIR.Fluxity
{
    public sealed class FeatureObserverAggregate : Dependent, IDisposable
    {
        private readonly List<Action> _releaseActions = new();
        private IStore _store;

        public event Action OnAnyStateChanged;

        public void Inject(IStore store) => _store = store;

        public IFeatureView<TState> Bind<TState>()
            where TState : struct
        {
            var newObserver = _store.GetFeatureObservable<TState>();
            void RouteToAny(TState _) { OnAnyStateChanged?.Invoke(); }
            newObserver.OnStateChanged += RouteToAny;
            _releaseActions.Add(() => newObserver.OnStateChanged -= RouteToAny);
            return newObserver;
        }

        public void Dispose()
        {
            foreach (var item in _releaseActions)
                item?.Invoke();
            OnAnyStateChanged = null;
        }
    }
}