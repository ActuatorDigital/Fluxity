using AIR.Flume;

namespace AIR.Fluxity
{
    public sealed class FeaturePresenterBinding<TState> : Dependent, IFeaturePresenterBinding<TState>
        where TState : struct
    {
        private readonly IPresenter _presenter;
        private IFeatureView<TState> _featureView;

        public FeaturePresenterBinding(IPresenter presenter)
            => _presenter = presenter;

        public TState CurrentState { get => _featureView.State; }

        public void Inject(IStore store)
        {
            _featureView = store.GetFeatureView<TState>();
            _featureView.OnStateChanged += OnStateChanged;
        }

        public void Dispose()
        {
            if (_featureView != null)
            {
                _featureView.OnStateChanged -= OnStateChanged;
                _featureView = null;
            }
        }

        private void OnStateChanged(TState state)
            => _presenter.Display();
    }
}