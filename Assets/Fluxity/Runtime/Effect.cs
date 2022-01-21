using AIR.Flume;

namespace AIR.Fluxity
{
    public abstract class Effect<TCommand> : Dependent, IEffect<TCommand>
        where TCommand : ICommand
    {
        public abstract void DoEffect(TCommand command);

        public void DoEffect(ICommand command)
            => DoEffect((TCommand)command);

        protected IDispatcher Dispatcher { get; private set; }

        protected Effect(IDispatcher dispatcher)
            => Dispatcher = dispatcher;
    }
}