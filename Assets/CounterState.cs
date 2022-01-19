using AIR.Fluxity;
using UnityEngine;

public struct CounterState
{
    public int CurrentCount;
}

public class IncrementCountCommand : ICommand
{
}

//Alternate reducer
public class CounterReducerAlt : Reducer<CounterState, IncrementCountCommand>
{
    public override CounterState Reduce(CounterState state, IncrementCountCommand command)
    {
        return new CounterState{CurrentCount = state.CurrentCount + 1};
    }
}

public static class CounterReducer
{
    public static CounterState Reduce(CounterState state, IncrementCountCommand command)
    {
        return new CounterState { CurrentCount = state.CurrentCount + 1 };
    }
}

public class CounterEffect : Effect<IncrementCountCommand>
{
    public CounterEffect(IDispatcher dispatcher) : base(dispatcher) { }

    public override void DoEffect(IncrementCountCommand command)
    {
        Debug.Log("Increment Command made.");
    }
}