using AIR.Fluxity;
using UnityEngine;

namespace Examples.Countdown
{
    [DefaultExecutionOrder(1)]
    public class FluxityExampleInitializer : FluxityInitializer
    {
        [SerializeField] private float uInitialCountdown = 10;

        protected override void Initialize()
        {
            CreateReducer<CountdownState, StartCountdownCommand>(CountdownReducer.StartCountDown);
            CreateReducer<CountdownState, StopCountdownCommand>(CountdownReducer.StopCountDown);
        }

        protected override void PostInitialize(IDispatcher dispatcher)
        {
            var command = new StartCountdownCommand { Seconds = uInitialCountdown };
            dispatcher.Dispatch(command);
        }
    }
}