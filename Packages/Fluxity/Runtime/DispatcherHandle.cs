using AIR.Flume;

namespace AIR.Fluxity
{
    public class DispatcherHandle : Dependent
    {
        private IDispatcher _dispatcher;

        public void Inject(IDispatcher dispatcher)
            => _dispatcher = dispatcher;

        public void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand
            => _dispatcher.Dispatch(command);
    }
}