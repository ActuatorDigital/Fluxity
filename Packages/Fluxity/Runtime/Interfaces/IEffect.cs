namespace AIR.Fluxity
{
    public interface IEffect<TCommand> : IEffect
        where TCommand : ICommand
    {
        void DoEffect(TCommand command);
    }

    public interface IEffect
    {
        void DoEffect(ICommand command);
    }
}