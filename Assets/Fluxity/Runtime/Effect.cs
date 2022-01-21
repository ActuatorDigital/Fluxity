using AIR.Flume;

namespace AIR.Fluxity
{
    public abstract class Effect<TCommand> : Dependent, IEffect<TCommand>
        where TCommand : ICommand
    {
        protected Effect(IDispatcher dispatcher)
            => Dispatcher = dispatcher;

        protected IDispatcher Dispatcher { get; private set; }

        public abstract void DoEffect(TCommand command);

        public void DoEffect(ICommand command)
            => DoEffect((TCommand)command);
    }
}