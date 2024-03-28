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

    [Test]
    public void Dispatch_WhenMatchingEffectRegistered_ShouldInvoke()
    {
        var payloadVal = 3;
        var command = new DummyCommand() { Payload = payloadVal };
        var dummyEffect = new DummyEffect();
        var effect = new EffectBinding<DummyCommand>(dummyEffect.DoEffect);
        _dispatcher.RegisterEffect(effect);

        _dispatcher.Dispatch(command);

        Assert.AreEqual(payloadVal, dummyEffect.accumPayload);
    }

    [Test]
    public void Dispatch_WhenMatchingEffectRegisteredViaExtension_ShouldInvoke()
    {
        var payloadVal = 3;
        var command = new DummyCommand() { Payload = payloadVal };
        var dummyEffect = new DummyEffect();
        var effect = new EffectBinding<DummyCommand>(dummyEffect.DoEffect);
        _dispatcher.RegisterEffect(effect);

        _dispatcher.Dispatch(command);

        Assert.AreEqual(payloadVal, dummyEffect.accumPayload);
    }

    [Test]
    public void Dispatch_WhenNonMatchingEffectRegistered_ShouldNotInvoke()
    {
        var command = new OtherDummyCommand();
        var dummyEffect = new DummyEffect();
        var effect = new EffectBinding<DummyCommand>(dummyEffect.DoEffect);
        _dispatcher.RegisterEffect(effect);

        _dispatcher.Dispatch(command);

        Assert.AreEqual(0, dummyEffect.accumPayload);
    }

    [Test]
    public void Dispatch_WhenMatchingReducerAndEffectRegistered_ShouldInvoke()
    {
        var state = new DummyState() { value = 1 };
        _feature.SetState(state);
        var reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(DummyReducers.Reduce);
        _store.RegisterReducer(reducer);
        var payloadVal = 2;
        var command = new DummyCommand() { Payload = payloadVal };
        var dummyEffect = new DummyEffect();
        var effect = new EffectBinding<DummyCommand>(dummyEffect.DoEffect);
        _dispatcher.RegisterEffect(effect);

        _dispatcher.Dispatch(command);

        Assert.AreEqual(3, _feature.State.value);
        Assert.AreEqual(payloadVal, dummyEffect.accumPayload);
    }

    [Test]
    public void Dispatch_WhenMultipleMatchingReducersAndEffectsRegistered_ShouldInvokeAll()
    {
        var state = new DummyState() { value = 1 };
        _feature.SetState(state);
        var reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(DummyReducers.Reduce);
        //two of the same reducer
        _store.RegisterReducer(reducer);
        _store.RegisterReducer(reducer);
        var payloadVal = 1;
        var command = new DummyCommand() { Payload = payloadVal };
        var dummyEffect = new DummyEffect();
        var effect = new EffectBinding<DummyCommand>(dummyEffect.DoEffect);
        //two of the same effect
        _dispatcher.RegisterEffect(effect);
        _dispatcher.RegisterEffect(effect);

        _dispatcher.Dispatch(command);

        Assert.AreEqual(3, _feature.State.value);
        Assert.AreEqual(payloadVal * 2, dummyEffect.accumPayload);
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
    public void Dispatch_WhenEffectDispatchesSameButLimited_ShouldDispatchTwiceAndEffectInvokeTwice()
    {
        var count = 0;
        _feature.SetState(new DummyState());
        _staticDispatcher = _dispatcher;
        var dummyEffect = new DummyEffect();
        dummyEffect.Action = () =>
        {
            if (count < 2)
                _staticDispatcher.Dispatch(new DummyCommand());
        };
        var effect = new EffectBinding<DummyCommand>(dummyEffect.DoEffect);
        _dispatcher.RegisterEffect(effect);
        _dispatcher.OnDispatch += _ => count++;

        _dispatcher.Dispatch(new DummyCommand());

        Assert.AreEqual(2, count);
        Assert.AreEqual(2, dummyEffect.callCount);
    }

    [Test]
    public void Dispatch_WhenEffectDispatchesOther_ShouldDispatchTwiceAndEffectInvokeOnce()
    {
        var count = 0;
        _feature.SetState(new DummyState());
        _staticDispatcher = _dispatcher;
        var dummyEffect = new DummyEffect();
        dummyEffect.Action = () =>
        {
            _staticDispatcher.Dispatch(new OtherDummyCommand());
        };
        var effect = new EffectBinding<DummyCommand>(dummyEffect.DoEffect);
        _dispatcher.RegisterEffect(effect);
        _dispatcher.OnDispatch += _ => count++;

        _dispatcher.Dispatch(new DummyCommand());

        Assert.AreEqual(2, count);
        Assert.AreEqual(1, dummyEffect.callCount);
    }

    [Test]
    public void Dispatch_WhenEffectDispatchesSameCommandEndlessly_ShouldThrow()
    {
        var count = 0;
        _feature.SetState(new DummyState());
        _staticDispatcher = _dispatcher;
        var dummyEffect = new DummyEffect();
        dummyEffect.Action = () =>
        {
            _staticDispatcher.Dispatch(new DummyCommand());
        };
        var effect = new EffectBinding<DummyCommand>(dummyEffect.DoEffect);
        _dispatcher.RegisterEffect(effect);
        _dispatcher.OnDispatch += _ => count++;


        void Act() => _dispatcher.Dispatch(new DummyCommand());

        Assert.Throws<DispatcherException>(Act);
    }

    private static DummyState DispatchDuringReduce(DummyState state, DummyCommand command)
    {
        _staticDispatcher.Dispatch(new OtherDummyCommand());
        return state;
    }

    [Test]
    public void Dispatch_WhenEffectDispatches_ShouldNotThrow()
    {
        var effect = new EffectBinding<DummyCommand>((DummyCommand dc, IDispatcher dispatcher) => _dispatcher.Dispatch(new OtherDummyCommand()));
        _dispatcher.RegisterEffect(effect);

        void Act() => _dispatcher.Dispatch(new DummyCommand());

        Assert.DoesNotThrow(Act);
    }

    [Test]
    public void Dispatch_WhenEffectRegistered_ShouldReturn1()
    {
        var dummyEffect = new DummyEffect();
        var effect = new EffectBinding<DummyCommand>(dummyEffect.DoEffect);
        _dispatcher.RegisterEffect(effect);

        var res = _dispatcher.GetAllEffectCommandTypes();
        var innerRes = _dispatcher.GetAllEffectsForCommandType(res.First());

        Assert.AreEqual(1, res.Count);
        Assert.AreEqual(1, innerRes.Count);
    }

    [Test]
    public void Dispatch_WhenNoEffectsRegistered_ShouldReturn0()
    {
        var res = _dispatcher.GetAllEffectCommandTypes();

        Assert.AreEqual(0, res.Count);
    }
}