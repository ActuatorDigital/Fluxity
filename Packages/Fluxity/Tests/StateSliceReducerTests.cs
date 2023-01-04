using AIR.Fluxity;
using NUnit.Framework;
using System;

public class StateSliceReducerTests
{
    private struct LargeState
    {
        public int lastResp;
        public string msg;
        public int executionCount;
    }

    private class LargeStateFeature : Feature<LargeState>
    {
        public LargeStateFeature(LargeState state)
            : base(state)
        {
        }
    }

    private class LargeStateCommand : ICommand { public int response; }

    private class LargeStateRespReducer : Reducer<LargeState, LargeStateCommand>
    {
        public override LargeState Reduce(LargeState state, LargeStateCommand command)
        {
            state.lastResp = command.response;
            return state;
        }
    }
    private class LargeStateMessageReducer : Reducer<LargeState, LargeStateCommand>
    {
        public override LargeState Reduce(LargeState state, LargeStateCommand command)
        {
            state.msg = Guid.Empty.ToString();
            return state;
        }
    }
    private class LargeStateExecutionCountReducer : Reducer<LargeState, LargeStateCommand>
    {
        public override LargeState Reduce(LargeState state, LargeStateCommand command)
        {
            state.executionCount++;
            return state;
        }
    }

    [Test]
    public void Dispatch_WhenMultipleReducersMatchCommand_ShouldCallEachAndHaveResultedCombinedState()
    {
        var dispatcher = new Dispatcher();
        var store = new Store();
        dispatcher.Inject(store);
        var feature = new LargeStateFeature(default);
        store.AddFeature(feature);
        var payloadVal = 3;
        var command = new LargeStateCommand() { response = payloadVal };
        var respReducer = new LargeStateRespReducer();
        feature.Register(respReducer);
        var msgReduer = new LargeStateMessageReducer();
        feature.Register(msgReduer);
        var exeCountReduer = new LargeStateExecutionCountReducer();
        feature.Register(exeCountReduer);

        dispatcher.Dispatch(command);

        Assert.AreEqual(payloadVal, feature.State.lastResp);
        Assert.AreEqual(Guid.Empty.ToString(), feature.State.msg);
        Assert.AreEqual(1, feature.State.executionCount);
    }

    [Test]
    public void Dispatch_WhenMultipleReducersMatchCommand_ShouldOnlyInvokeStateChangeOnce()
    {
        var changeCount = 0;
        var dispatcher = new Dispatcher();
        var store = new Store();
        dispatcher.Inject(store);
        var feature = new LargeStateFeature(default);
        feature.OnStateChanged += (x) => changeCount++;
        store.AddFeature(feature);
        var payloadVal = 3;
        var command = new LargeStateCommand() { response = payloadVal };
        var respReducer = new LargeStateRespReducer();
        feature.Register(respReducer);
        var msgReduer = new LargeStateMessageReducer();
        feature.Register(msgReduer);
        var exeCountReduer = new LargeStateExecutionCountReducer();
        feature.Register(exeCountReduer);

        dispatcher.Dispatch(command);

        Assert.AreEqual(1, changeCount);
    }
}
