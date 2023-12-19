using System;
using AIR.Flume;

namespace AIR.Fluxity
{
    public sealed class FeatureObserver<TState> : Dependent, IFeatureView<TState>, IDisposable
        where TState : struct
    {
        private IFeatureObservable<TState> _featureView;

        public TState State => _featureView.State;

        public event Action<TState> OnStateChanged;

        public void Inject(IStore store)
        {
            _featureView = store.GetFeatureObservable<TState>();
            _featureView.OnStateChanged += InvokeOnStateChanged;
        }

        public void Dispose()
        {
            if (_featureView != null)
            {
                _featureView.OnStateChanged -= InvokeOnStateChanged;
                _featureView = null;
            }
        }

        private void InvokeOnStateChanged(TState state)
        {
            OnStateChanged?.Invoke(state);
        }
    }
}