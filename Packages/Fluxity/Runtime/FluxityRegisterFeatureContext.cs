namespace AIR.Fluxity
{
    public sealed class FluxityRegisterFeatureContext<TState> : FluxityRegisterContext
        where TState : struct
    {
        private readonly Feature<TState> _feature;

        public FluxityRegisterFeatureContext(
            IStore store,
            IDispatcher dispatcher,
            Feature<TState> feature)
            : base(store, dispatcher)
        {
            _feature = feature;
        }

        public FluxityRegisterFeatureContext<TState> Reducer<TCommand>(IReducer<TState, TCommand>.ReduceDelegate pureFunctionReducer)
            where TCommand : ICommand
        {
            var reducer = new PureFunctionReducerBinder<TState, TCommand>(pureFunctionReducer);
            _feature.Register(reducer);
            return this;
        }
    }
}