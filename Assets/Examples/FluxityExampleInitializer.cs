using AIR.Fluxity;

public class FluxityExampleInitializer : FluxityInitializer
{
    protected override void Initialize()
    {
        CreateReducer<CounterState, IncrementCountCommand>(CounterReducer.Reduce);
        CreateEffect<CounterEffect, IncrementCountCommand>();
    }
}