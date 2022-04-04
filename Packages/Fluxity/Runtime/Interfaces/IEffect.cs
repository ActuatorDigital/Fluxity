using System.Threading.Tasks;

namespace AIR.Fluxity
{
    public interface IEffect<TCommand> : IEffect
        where TCommand : ICommand
    {
        Task DoEffect(TCommand command);
    }

    public interface IEffect
    {
        Task DoEffect(ICommand command);
    }
}