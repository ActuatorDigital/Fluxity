using AIR.Fluxity;

public class FluxityExampleInitializer : FluxityInitializer
{
    protected override void Initialise()
    {
        CreateReducer<CounterState, IncrementCountCommand>(CounterReducer.Reduce);
        CreateEffect<CounterEffect, IncrementCountCommand>();
    }
}