using AIR.Flume;

namespace AIR.Fluxity
{
    public sealed class FeaturePresenterBinding<TState> : Dependent, IFeaturePresenterBinding<TState>
        where TState : struct
    {
        private readonly IPresenter _presenter;
        private IFeature<TState> _feature;

        public FeaturePresenterBinding(IPresenter presenter)
            => _presenter = presenter;

        public TState CurrentState { get => _feature.State; }

        public void Inject(IStore store)
        {
            _feature = store.GetFeature<TState>();
            _feature.OnStateChanged += OnStateChanged;
        }

        public void Dispose()
        {
            if (_feature != null)
            {
                _feature.OnStateChanged -= OnStateChanged;
                _feature = null;
            }
        }

        private void OnStateChanged(TState state)
            => _presenter.Display();
    }
}