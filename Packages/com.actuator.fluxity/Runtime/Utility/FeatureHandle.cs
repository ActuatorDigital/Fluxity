using AIR.Flume;

namespace AIR.Fluxity
{
    public sealed class FeatureHandle<TState> : Dependent, IFeatureView<TState>
        where TState : struct
    {
        private IFeatureObservable<TState> _feature;

        public TState State => _feature.State;

        public void Inject(IStore store)
        {
            _feature = store.GetFeatureObservable<TState>();
        }
    }
}