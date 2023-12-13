using System.Reflection;
using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class ReducerTests
{
    private PureFunctionReducerBinder<DummyState, DummyCommand> _reducer;
    private DummyDoubleReducer _doubleReducer;

    private class DummyDoubleReducer : Reducer<DummyState, DummyCommand>
    {
        public override DummyState Reduce(DummyState state, DummyCommand command)
        {
            return new DummyState() { value = state.value * 2 };
        }

        public override MethodInfo ReducerBindingInfo()
        {
            throw new System.NotImplementedException();
        }
    }

    [SetUp]
    public void SetUp()
    {
        _reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(DummyReducers.Reduce);
        _doubleReducer = new DummyDoubleReducer();
    }

    [Test]
    public void Reduce_WhenGivenIncorrectType_ShouldThrow()
    {
        var wrongCommandType = new OtherDummyCommand();

        void Act()
        {
            _reducer.Reduce(new  DummyState(), wrongCommandType);
        }

        Assert.Throws<System.InvalidCastException>(Act);
    }

    [Test]
    public void ProcessReducers_WhenGivenCorrectType_ShouldCallWithExpectedPayloadValue()
    {
        var state = new DummyState() { value = 1 };
        var feature = new Feature<DummyState>(default);
        feature.SetState(state);
        var payloadVal = 3;
        var correctCommandType = new DummyCommand() { Payload = payloadVal };
        feature.Register(_reducer);

        feature.ProcessReducers(correctCommandType);

        Assert.AreEqual(4, feature.State.value);
    }

    [Test]
    public void ProcessReducers_WhenGivenMultipleOverlappingReducers_ShouldCallBindingOrder()
    {
        var initial = 1;
        var expected = 8;
        var payloadVal = 3;
        var state = new DummyState() { value = initial };
        var feature = new Feature<DummyState>(default);
        feature.SetState(state);
        var correctCommandType = new DummyCommand() { Payload = payloadVal };
        feature.Register(_reducer);
        feature.Register(_doubleReducer);

        feature.ProcessReducers(correctCommandType);

        Assert.AreEqual(expected, feature.State.value);
    }

    [Test]
    public void ProcessReducers_WhenGivenMultipleOverlappingReducersReversed_ShouldCallBindingOrder()
    {
        var initial = 1;
        var expected = 5;
        var payloadVal = 3;
        var state = new DummyState() { value = initial };
        var feature = new Feature<DummyState>(default);
        feature.SetState(state);
        var correctCommandType = new DummyCommand() { Payload = payloadVal };
        feature.Register(_doubleReducer);
        feature.Register(_reducer);

        feature.ProcessReducers(correctCommandType);

        Assert.AreEqual(expected, feature.State.value);
    }
}