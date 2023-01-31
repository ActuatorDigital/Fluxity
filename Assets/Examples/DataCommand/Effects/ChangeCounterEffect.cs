using AIR.Flume;
using AIR.Fluxity;

namespace Examples.DataCommand
{
    public class ChangeCounterEffect : Dependent
    {
        private ISomeService _someService;

        public void Inject(ISomeService someService)
        {
            _someService = someService;
        }

        public void DoEffect(ChangeCountCommand command, IDispatcher dispatcher)
        {
            _someService.DoSomething($"Count changed by {command.Delta}.");
        }
    }
}