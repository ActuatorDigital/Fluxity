using AIR.Flume;

namespace AIR.Fluxity
{
    public sealed class StatePresenterBinding<TState> : Dependent, IStatePresenterBinding<TState>
        where TState : struct
    {
        private readonly IPresenter _drawer;
        private IFeature<TState> _feature;

        public StatePresenterBinding(IPresenter presenter)
            => _drawer = presenter;

        public TState CurrentState { get => _feature.State; }

        public void Inject(IFeature<TState> feature)
        {
            _feature = feature;
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
            => _drawer.Display();
    }
}