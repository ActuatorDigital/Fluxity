using System;
using AIR.Flume;

namespace AIR.Fluxity
{
    public sealed class FeatureObserver<TState> : Dependent, IFeatureView<TState>, IDisposable
        where TState : struct
    {
        private IFeatureObservable<TState> _featureObservable;

        public TState State => _featureObservable.State;

        public event Action<TState> OnStateChanged;

        public void Inject(IStore store)
        {
            _featureObservable = store.GetFeatureObservable<TState>();
            _featureObservable.OnStateChanged += InvokeOnStateChanged;
        }

        public void Dispose()
        {
            if (_featureObservable != null)
            {
                _featureObservable.OnStateChanged -= InvokeOnStateChanged;
                _featureObservable = null;
            }
        }

        private void InvokeOnStateChanged(TState state)
        {
            OnStateChanged?.Invoke(state);
        }
    }
}