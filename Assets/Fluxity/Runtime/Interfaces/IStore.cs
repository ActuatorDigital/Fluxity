namespace AIR.Fluxity
{
    public interface IStore
    {
        void ProcessCommand<TCommand>(TCommand command) where TCommand : ICommand;

        void AddFeature<TState>(IFeature<TState> feature) where TState : struct;

        void Register<TState, TCommand>(IReducer<TState, TCommand> reducer)
            where TState : struct
            where TCommand : ICommand;
    }
}