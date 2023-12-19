using AIR.Fluxity;
using UnityEngine;

namespace Examples.Spinner
{
    [DefaultExecutionOrder(1)]
    public class SpinExampleInitializer : FluxityInitializer
    {
        protected override void PostInitialize(IDispatcher dispatcher)
        {
            // Initial state that object starts out spinning.
            var command = new StartSpinCommand { DegreesPerSecond = 270f };
            dispatcher.Dispatch(command);
        }
    }
}