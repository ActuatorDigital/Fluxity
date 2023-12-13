using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;
using System;
using System.Linq;

public class FeatureTests
{
    private Feature<DummyState> _feature;

    [SetUp]
    public void SetUp()
    {
        _feature = new Feature<DummyState>(default);
    }

    [Test]
    public void Register_WhenCalledWithIncorrectType_ShouldThrow()
    {
        var reducer = new PureFunctionReducerBinder<OtherDummyState, OtherDummyCommand>(OtherDummyReducer.Reduce);

        void Act() => _feature.Register(reducer);

        Assert.Throws<InvalidCastException>(Act);
    }

    [Test]
    public void Register_WhenCalledWithCorrectType_ShouldNotThrow()
    {
        var reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(DummyReducers.Reduce);

        void Act() => _feature.Register(reducer);

        Assert.DoesNotThrow(Act);
    }

    [Test]
    public void ProcessReducers_WhenNoReducersRegistered_ShouldNotThrow()
    {
        var correctCommandType = new DummyCommand();

        void Act() => _feature.ProcessReducers(correctCommandType);

        Assert.DoesNotThrow(Act);
    }

    [Test]
    public void OnStateChanged_WhenSetStateCalled_ShouldInvoke()
    {
        var callCount = 0;

        _feature.OnStateChanged += (_) => callCount++;
        _feature.SetState(new DummyState());

        Assert.AreEqual(1, callCount);
    }

    [Test]
    public void OnStateChanged_WhenSetStateCalledMultipleTimes_ShouldInvokeMultipleTimes()
    {
        var callCount = 0;
        var setIterations = 5;

        _feature.OnStateChanged += (_) => callCount++;
        for (int i = 0; i < setIterations; i++)
        {
            _feature.SetState(new DummyState());
        }

        Assert.AreEqual(setIterations, callCount);
    }

    [Test]
    public void OnStateChanged_WhenSetStateCalledAndMultipleListeners_ShouldInvokeEachListenerOnce()
    {
        var callCountA = 0;
        var callCountB = 0;

        _feature.OnStateChanged += (_) => callCountA++;
        _feature.OnStateChanged += (_) => callCountB++;
        _feature.SetState(new DummyState());

        Assert.AreEqual(1, callCountA);
        Assert.AreEqual(1, callCountB);
    }

    [Test]
    public void OnStateChanged_WhenSetStateCalled_ShouldInvokeWithSpecificObject()
    {
        DummyState returned = default;
        var sent = new DummyState() { value = 3 };

        _feature.OnStateChanged += (state) => returned = state;
        _feature.SetState(sent);

        Assert.AreEqual(returned, sent);
    }

    [Test]
    public void GetHandled_WhenOneBound_ShouldReturnExpectedOne()
    {
        var reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(DummyReducers.Reduce);
        _feature.Register(reducer);

        var res = _feature.GetAllHandledCommandTypes();
        var resInner = _feature.GetAllReducersForCommand(res.First());
        var innerBindingInfo = resInner.First().ReducerBindingInfo();

        Assert.AreEqual(1, res.Count);
        StringAssert.AreEqualIgnoringCase(nameof(DummyCommand), res.First().Name);
        StringAssert.AreEqualIgnoringCase(nameof(DummyReducers.Reduce), innerBindingInfo.Name);
        StringAssert.AreEqualIgnoringCase(nameof(DummyReducers), innerBindingInfo.DeclaringType.Name);
    }

    [Test]
    public void GetHandled_WhenNoneBound_ShouldReturnZero()
    {
        var res = _feature.GetAllHandledCommandTypes();

        Assert.AreEqual(0, res.Count);
    }
}