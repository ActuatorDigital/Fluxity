namespace AIR.Fluxity
{
    public interface IDispatcher
    {
        public void RegisterEffect<TCommand>(IEffect<TCommand> effect)
            where TCommand : ICommand;

        public void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}