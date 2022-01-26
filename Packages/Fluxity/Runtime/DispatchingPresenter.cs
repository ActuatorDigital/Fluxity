namespace AIR.Fluxity
{
    public abstract class DispatchingPresenter : Presenter
    {
        private IDispatcher _dispatcher;

        public void Inject(IDispatcher dispatcher)
            => _dispatcher = dispatcher;

        public void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand
            => _dispatcher.Dispatch(command);
    }
}