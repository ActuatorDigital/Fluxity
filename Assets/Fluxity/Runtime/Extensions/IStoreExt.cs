namespace AIR.Fluxity
{
    public static class IStoreExt
    {
        public static IReducer CreateAndRegister<TState, TCommand>(
            this IStore self,
            IReducer<TState, TCommand>.ReduceDelegate pureFunctionReducer)
            where TState : struct
            where TCommand : ICommand
        {
            var reducer = new PureFunctionReducerBinder<TState, TCommand>(pureFunctionReducer);
            self.Register(reducer);
            return reducer;
        }
    }
}