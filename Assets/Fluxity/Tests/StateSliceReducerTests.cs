﻿using AIR.Fluxity;
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

    private class LargeStateFeature : Feature<LargeState> { }

    private class LargeStateCommand : ICommand { public int response; }

    private class LargetStateRespReducer : Reducer<LargeState, LargeStateCommand>
    {
        public override LargeState Reduce(LargeState state, LargeStateCommand command)
        {
            state.lastResp = command.response;
            return state;
        }
    }
    private class LargetStateMessageReducer : Reducer<LargeState, LargeStateCommand>
    {
        public override LargeState Reduce(LargeState state, LargeStateCommand command)
        {
            state.msg = Guid.Empty.ToString();
            return state;
        }
    }
    private class LargetStateExecutionCountReducer : Reducer<LargeState, LargeStateCommand>
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
        var feature = new LargeStateFeature();
        store.AddFeature(feature);
        var payloadVal = 3;
        var command = new LargeStateCommand() { response = payloadVal };
        var respReducer = new LargetStateRespReducer();
        feature.Register(respReducer);
        var msgReduer = new LargetStateMessageReducer();
        feature.Register(msgReduer);
        var exeCountReduer = new LargetStateExecutionCountReducer();
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
        var feature = new LargeStateFeature();
        feature.OnStateChanged += (x) => changeCount++; 
        store.AddFeature(feature);
        var payloadVal = 3;
        var command = new LargeStateCommand() { response = payloadVal };
        var respReducer = new LargetStateRespReducer();
        feature.Register(respReducer);
        var msgReduer = new LargetStateMessageReducer();
        feature.Register(msgReduer);
        var exeCountReduer = new LargetStateExecutionCountReducer();
        feature.Register(exeCountReduer);

        dispatcher.Dispatch(command);

        Assert.AreEqual(1, changeCount);
    }
}
