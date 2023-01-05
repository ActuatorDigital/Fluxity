using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class PureFunctionReducerTests
{
    private PureFunctionReducerBinder<DummyState, DummyCommand> _reducer;

    private static class DummyPureFunctionReducerInvalidForms
    {
        public static DummyState ReduceWrongArgCount(DummyState state, DummyCommand command, int extra)
        {
            return new DummyState() { value = state.value + command.payload };
        }

        public static int ReduceWrongReturn(DummyState state, DummyCommand command)
        {
            return 1;
        }

        public static DummyState ReduceWrongState(int state, DummyCommand command)
        {
            return new DummyState();
        }

        public static DummyState ReduceWrongCommand(DummyState state, int command)
        {
            return new DummyState();
        }
    }

    private class DummyPureFunctionReducerNonStatic
    {
        public DummyState ReduceNonStatic(DummyState state, DummyCommand command)
        {
            return new DummyState() { value = state.value + command.payload };
        }
    }

    [Test]
    public void DoReduce_WhenGivenIncorrectType_ShouldThrow()
    {
        _reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(typeof(DummyPureFunctionReducer).GetMethod("Reduce"));
        var wrongCommandType = new OtherDummyCommand();

        void Act()
        {
            _reducer.Reduce(new DummyState(), wrongCommandType);
        }

        Assert.Throws<System.InvalidCastException>(Act);
    }

    [Test]
    public void DoReduce_WhenGivenCorrectType_ShouldCallWithExpectedPayloadValue()
    {
        _reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(typeof(DummyPureFunctionReducer).GetMethod("Reduce"));
        var state = new DummyState() { value = 1 };
        var feature = new DummyFeature(default);
        feature.SetState(state);
        var payloadVal = 3;
        var correctCommandType = new DummyCommand() { payload = payloadVal };
        feature.Register(_reducer);

        feature.ProcessReducers(correctCommandType);

        Assert.AreEqual(4, feature.State.value);
    }

    [Test]
    public void DoReduce_WhenConstructedFromFuncAndGivenCorrectType_ShouldCallWithExpectedPayloadValue()
    {
        _reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(DummyPureFunctionReducer.Reduce);
        var state = new DummyState() { value = 1 };
        var feature = new DummyFeature(default);
        feature.SetState(state);
        var payloadVal = 3;
        var correctCommandType = new DummyCommand() { payload = payloadVal };
        feature.Register(_reducer);

        feature.ProcessReducers(correctCommandType);

        Assert.AreEqual(4, feature.State.value);
    }

    [Test]
    public void Ctor_WhenConstructedFromInvalidFunc_ShouldThrow()
    {
        void Act() => _reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(new DummyPureFunctionReducerNonStatic().ReduceNonStatic);

        var exception = Assert.Throws<ReducerException>(Act);
        Assert.AreEqual("Expected static got instance. Invalid method on reducer 'ReduceNonStatic'.", exception.Message);
    }

    [Test]
    public void Ctor_WhenConstructedWithInvalidReturn_ShouldThrow()
    {
        var EXPECTED_ERR_MSG = $"Expected '{typeof(DummyState)}' but got '{typeof(int)}'. Invalid return type on reducer '{nameof(DummyPureFunctionReducerInvalidForms.ReduceWrongReturn)}'.";

        void Act() => _reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(typeof(DummyPureFunctionReducerInvalidForms).GetMethod(nameof(DummyPureFunctionReducerInvalidForms.ReduceWrongReturn)));

        var exception = Assert.Throws<ReducerException>(Act);
        Assert.AreEqual(EXPECTED_ERR_MSG, exception.Message);
    }

    [Test]
    public void Ctor_WhenConstructedWithInvalidState_ShouldThrow()
    {
        var EXPECTED_ERR_MSG = $"Expected '{typeof(DummyState)}' but got '{typeof(int)}'. Invalid state arg type on reducer '{nameof(DummyPureFunctionReducerInvalidForms.ReduceWrongState)}'.";

        void Act() => _reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(typeof(DummyPureFunctionReducerInvalidForms).GetMethod(nameof(DummyPureFunctionReducerInvalidForms.ReduceWrongState)));

        var exception = Assert.Throws<ReducerException>(Act);
        Assert.AreEqual(EXPECTED_ERR_MSG, exception.Message);
    }

    [Test]
    public void Ctor_WhenConstructedWithInvalidCommand_ShouldThrow()
    {
        var EXPECTED_ERR_MSG = $"Expected '{typeof(DummyCommand)}' but got '{typeof(int)}'. Invalid command arg type on reducer '{nameof(DummyPureFunctionReducerInvalidForms.ReduceWrongCommand)}'.";

        void Act() => _reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(typeof(DummyPureFunctionReducerInvalidForms).GetMethod(nameof(DummyPureFunctionReducerInvalidForms.ReduceWrongCommand)));

        var exception = Assert.Throws<ReducerException>(Act);
        Assert.AreEqual(EXPECTED_ERR_MSG, exception.Message);
    }

    [Test]
    public void Ctor_WhenConstructedWithInvalidNumberOfArgs_ShouldThrow()
    {
        void Act() => _reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(typeof(DummyPureFunctionReducerInvalidForms).GetMethod(nameof(DummyPureFunctionReducerInvalidForms.ReduceWrongArgCount)));

        var exception = Assert.Throws<ReducerException>(Act);
        Assert.AreEqual("Expected '2' but got '3'. Invalid argument count on reducer 'ReduceWrongArgCount'.", exception.Message);
    }
}
