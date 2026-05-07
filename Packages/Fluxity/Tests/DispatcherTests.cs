using System.Collections.Generic;
using System.Linq;
using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class DispatcherTests
{
    private Store _store;
    private Dispatcher _dispatcher;
    private Feature<DummyState> _feature;
    private static Dispatcher _staticDispatcher;

    [SetUp]
    public void SetUp()
    {
        _store = new Store();
        _dispatcher = new Dispatcher(_store);
        _feature = new Feature<DummyState>(default);
        _store.AddFeature(_feature);
    }

    [Test]
    public void Dispatch_WhenNothingRegistered_ShouldNotThrow()
    {
        void Act() => _dispatcher.Dispatch(new DummyCommand());

        Assert.DoesNotThrow(Act);
    }

    [Test]
    public void Dispatch_WhenMatchingReducerRegistered_ShouldInvoke()
    {
        var state = new DummyState() { value = 1 };
        _feature.SetState(state);
        var payloadVal = 3;
        var command = new DummyCommand() { Payload = payloadVal };
        var reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(DummyReducers.Reduce);
        _feature.Register(reducer);

        _dispatcher.Dispatch(command);

        Assert.AreEqual(4, _feature.State.value);
    }

    [Test]
    public void Dispatch_WhenNonMatchingReducerRegistered_ShouldNotInvoke()
    {
        var state = new DummyState() { value = 1 };
        _feature.SetState(state);
        var reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(DummyReducers.Reduce);
        _feature.Register(reducer);

        _dispatcher.Dispatch(new OtherDummyCommand());

        Assert.AreEqual(1, _feature.State.value);
    }

    private static DummyState DispatchDuringReduce(DummyState state, DummyCommand command)
    {
        _staticDispatcher.Dispatch(new OtherDummyCommand());
        return state;
    }

    [Test]
    public void Dispatch_WhenReducerDispatches_ShouldDispatchTwice()
    {
        var count = 0;
        _feature.SetState(new DummyState());
        _staticDispatcher = _dispatcher;
        //this is a hack that we don't actually want to allow but now we queue so we can ensure the following thing happens
        var reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(DispatchDuringReduce);
        _feature.Register(reducer);
        _dispatcher.OnDispatch += _ => count++;

        _dispatcher.Dispatch(new DummyCommand());

        Assert.AreEqual(2, count);
    }

    [Test]
    public void Dispatch_WhenCallbackDispatchesSameButLimited_ShouldDispatchTwiceAndInvokeTwice()
    {
        var count = 0;
        var preCount = 0;
        var postCount = 0;
        _feature.SetState(new DummyState());
        _dispatcher.OnDispatch += _ => count++;
        _store.AddPreAction((DummyCommand cmd) =>
        {
            if (postCount == 0)
            {
                preCount++;
            }
        });
        _store.AddPostAction((DummyCommand cmd) =>
        {
            postCount++;
            if (count >= 2) return;
            _dispatcher.Dispatch(new DummyCommand());
        });

        _dispatcher.Dispatch(new DummyCommand());

        Assert.AreEqual(2, count);
        Assert.AreEqual(1, preCount);
        Assert.AreEqual(2, postCount);
    }

    [Test]
    public void Dispatch_WhenEndlessNestedDispatch_ShouldThrow()
    {
        _feature.SetState(new DummyState());
        _store.AddPostAction((DummyCommand cmd) =>
        {
            _dispatcher.Dispatch(new DummyCommand());
        });

        void Action ()
        {
            _dispatcher.Dispatch(new DummyCommand());
        }

        Assert.Throws<DispatcherException>(Action);
    }

    [Test]
    public void Dispatch_WhenEffectDispatchesOther_ShouldDispatchTwiceAndEffectInvokeOnce()
    {
        var count = 0;
        var postCount = 0;
        _feature.SetState(new DummyState());
        _dispatcher.OnDispatch += _ => count++;
        _store.AddPostAction((DummyCommand cmd) =>
        {
            postCount++;
            _dispatcher.Dispatch(new OtherDummyCommand());
        });

        _dispatcher.Dispatch(new DummyCommand());

        Assert.AreEqual(2, count);
        Assert.AreEqual(1, postCount);
    }
}