using AIR.Flume;

namespace AIR.Fluxity
{
    public sealed class StatePresenterBinding<TState> : Dependent, IStatePresenterBinding<TState>
        where TState : struct
    {
        private IFeature<TState> _feature;
        private readonly IPresenter drawer;

        public TState CurrentState { get => _feature.State; }

        public StatePresenterBinding(IPresenter presenter)
            => drawer = presenter;

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
            => drawer.Display();
    }
}