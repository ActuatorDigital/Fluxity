﻿using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class DispatcherTests
{
    private Store _store;
    private Dispatcher _dispatcher;
    private DummyFeature _feature;

    [SetUp]
    public void SetUp()
    {
        _store = new Store();
        _dispatcher = new Dispatcher();
        _dispatcher.Inject(_store);
        _feature = new DummyFeature();
        _feature.Inject(_store);
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
        var command = new DummyCommand() { payload = payloadVal };
        var reducer = new DummyReducer();
        _feature.Register(reducer);

        _dispatcher.Dispatch(command);

        Assert.AreEqual(4, _feature.State.value);
    }

    [Test]
    public void Dispatch_WhenNonMatchingReducerRegistered_ShouldNotInvoke()
    {
        var state = new DummyState() { value = 1 };
        _feature.SetState(state);
        var reducer = new DummyReducer();
        _feature.Register(reducer);

        _dispatcher.Dispatch(new OtherDummyCommand());

        Assert.AreEqual(1, _feature.State.value);
    }

    [Test]
    public void Dispatch_WhenMatchingEffectRegistered_ShouldInvoke()
    {
        var payloadVal = 3;
        var command = new DummyCommand() { payload = payloadVal };
        var effect = new DummyEffect(_dispatcher);
        _dispatcher.RegisterEffect(effect);

        _dispatcher.Dispatch(command);

        Assert.AreEqual(payloadVal, effect.accumPayload);
    }

    [Test]
    public void Dispatch_WhenMatchingEffectRegisteredViaExtension_ShouldInvoke()
    {
        var payloadVal = 3;
        var command = new DummyCommand() { payload = payloadVal };
        var effect = _dispatcher.CreateAndRegister<DummyEffect, DummyCommand>();

        _dispatcher.Dispatch(command);

        Assert.AreEqual(payloadVal, effect.accumPayload);
    }

    [Test]
    public void Dispatch_WhenNonMatchingEffectRegistered_ShouldNotInvoke()
    {
        var command = new OtherDummyCommand();
        var effect = new DummyEffect(_dispatcher);
        _dispatcher.RegisterEffect(effect);

        _dispatcher.Dispatch(command);

        Assert.AreEqual(0, effect.accumPayload);
    }

    [Test]
    public void Dispatch_WhenMatchingReducerAndEffectRegistered_ShouldInvoke()
    {
        var state = new DummyState() { value = 1 };
        _feature.SetState(state);
        var reducer = new DummyReducer();
        _store.RegisterReducer(reducer);
        var payloadVal = 2;
        var command = new DummyCommand() { payload = payloadVal };
        var effect = new DummyEffect(_dispatcher);
        _dispatcher.RegisterEffect(effect);

        _dispatcher.Dispatch(command);

        Assert.AreEqual(3, _feature.State.value);
        Assert.AreEqual(payloadVal, effect.accumPayload);
    }

    [Test]
    public void Dispatch_WhenMultipleMatchingReducersAndEffectsRegistered_ShouldInvokeAll()
    {
        var state = new DummyState() { value = 1 };
        _feature.SetState(state);
        var reducer = new DummyReducer();
        //two of the same reducer
        _store.RegisterReducer(reducer);
        _store.RegisterReducer(reducer);
        var payloadVal = 1;
        var command = new DummyCommand() { payload = payloadVal };
        var effect = new DummyEffect(_dispatcher);
        //two of the same effect
        _dispatcher.RegisterEffect(effect);
        _dispatcher.RegisterEffect(effect);

        _dispatcher.Dispatch(command);

        Assert.AreEqual(3, _feature.State.value);
        Assert.AreEqual(payloadVal * 2, effect.accumPayload);
    }

    [Test]
    public void Dispatch_WhenReducerDispatches_ShouldThrow()
    {
        _feature.SetState(new DummyState());
        IReducer reducer = new DummyDelegateReducer(() => _dispatcher.Dispatch(new OtherDummyCommand()));
        _feature.Register(reducer);

        void Act() => _dispatcher.Dispatch(new DummyCommand());

        Assert.Throws<DispatcherException>(Act);
    }

    [Test]
    public void Dispatch_WhenEffectDispatches_ShouldNotThrow()
    {
        var effect = new DummyDelegateEffect(
            () => _dispatcher.Dispatch(new OtherDummyCommand()),
            _dispatcher);
        _dispatcher.RegisterEffect(effect);

        void Act() => _dispatcher.Dispatch(new DummyCommand());
        
        Assert.DoesNotThrow(Act);
    }
}