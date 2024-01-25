using AIR.Flume;
using AIR.Fluxity;
using UnityEngine;

namespace Examples.Countdown
{
    public class FluxityExampleInitializer : FluxityInitializer
    {
        [SerializeField] private float uInitialCountdown = 10;

        public override void RegisterFluxity(FluxityRegisterContext context)
        {
            context
                .Feature(new CountdownState())
                    .Reducer<StartCountdownCommand>(CountdownReducer.StartCountDown)
                    .Reducer<StopCountdownCommand>(CountdownReducer.StopCountDown)
                ;
        }

        protected override void RegisterServices(FlumeServiceContainer container)
        {
        }

        protected override void PostInitialize(IDispatcher dispatcher)
        {
            var command = new StartCountdownCommand { Seconds = uInitialCountdown };
            dispatcher.Dispatch(command);
        }
    }
}