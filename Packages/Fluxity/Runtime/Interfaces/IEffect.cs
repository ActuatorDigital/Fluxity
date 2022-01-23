namespace AIR.Fluxity
{
    public interface IEffect<T> : IEffect
        where T : ICommand
    {
        void DoEffect(T command);
    }

    public interface IEffect
    {
        void DoEffect(ICommand command);
    }
}