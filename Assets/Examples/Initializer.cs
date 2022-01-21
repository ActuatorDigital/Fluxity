using AIR.Fluxity;

public class Initializer : FluxityInitializer
{
    protected override void Initialise()
    {
        CreateReducer<CounterState, IncrementCountCommand>(CounterReducer.Reduce);
        CreateEffect<CounterEffect, IncrementCountCommand>();
    }
}