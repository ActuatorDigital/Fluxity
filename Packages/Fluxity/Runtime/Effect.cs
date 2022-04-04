using System.Threading.Tasks;
using AIR.Flume;

namespace AIR.Fluxity
{
    public abstract class Effect<TCommand> : Dependent, IEffect<TCommand>
        where TCommand : ICommand
    {
        protected Effect(IDispatcher dispatcher)
            => Dispatcher = dispatcher;

        protected IDispatcher Dispatcher { get; private set; }

        public abstract Task DoEffect(TCommand command);

        public Task DoEffect(ICommand command)
            => DoEffect((TCommand)command);
    }
}