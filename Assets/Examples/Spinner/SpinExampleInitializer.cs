using AIR.Flume;
using AIR.Fluxity;

namespace Examples.Spinner
{
    public class SpinExampleInitializer : FluxityInitializer
    {
        public override void RegisterFluxity(FluxityRegisterContext context)
        {
            context
                .Feature(new SpinState())
                    .Reducer<StartSpinCommand>(SpinnerReducers.StartSpin)
                    .Reducer<StopSpinCommand>(SpinnerReducers.StopSpin)
                ;
        }

        protected override void RegisterServices(FlumeServiceContainer container)
        {
        }

        protected override void PostInitialize(IDispatcher dispatcher)
        {
            // Initial state that object starts out spinning.
            var command = new StartSpinCommand { DegreesPerSecond = 270f };
            dispatcher.Dispatch(command);
        }
    }
}