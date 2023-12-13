using System;

namespace AIR.Fluxity
{
    public class FluxityFeatureContext<TState> : FluxityFlumeRegisterContext
        where TState : struct
    {
        private readonly Feature<TState> _feature;
        public FluxityFeatureContext(FluxityFlumeRegisterContext context, Feature<TState> feature)
            : base(context.Store, context.Distpatcher, context.FlumeServiceContainer)
        {
            _feature = feature;
        }

        public FluxityFeatureContext<TState> BulkReducers(Action<FluxityFeatureContext<TState>> registerAll)
        {
            registerAll(this);
            return this;
        }

        public FluxityFeatureContext<TState> Reducer<TCommand>(IReducer<TState, TCommand>.ReduceDelegate pureFunctionReducer)
            where TCommand : ICommand
        {
            var reducer = new PureFunctionReducerBinder<TState, TCommand>(pureFunctionReducer);
            _feature.Register(reducer);
            return this;
        }
    }
}