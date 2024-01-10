using AIR.Flume;
using AIR.Fluxity;
using UnityEngine;

namespace Examples.Spinner
{
    public class SpinExampleInitializer : FluxityInitializer
    {
        public override void Register(FluxityRegisterContext context)
        {
            context
                .Feature(new SpinState())
                    .Reducer<StartSpinCommand>(SpinnerReducers.StartSpin)
                    .Reducer<StopSpinCommand>(SpinnerReducers.StopSpin)
                ;
        }

        protected override void Install(FlumeServiceContainer container)
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