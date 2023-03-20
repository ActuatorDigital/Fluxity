using System.Linq;
using AIR.Fluxity;
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
        _feature = new DummyFeature(default);
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
        var command = new DummyCommand() { payload = payloadVal };
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
        var reducer = new DummyReducer();
        _store.RegisterReducer(reducer);
        var payloadVal = 2;
        var command = new DummyCommand() { payload = payloadVal };
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
        var reducer = new DummyReducer();
        //two of the same reducer
        _store.RegisterReducer(reducer);
        _store.RegisterReducer(reducer);
        var payloadVal = 1;
        var command = new DummyCommand() { payload = payloadVal };
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

        var res =_dispatcher.GetAllEffectCommandTypes();
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