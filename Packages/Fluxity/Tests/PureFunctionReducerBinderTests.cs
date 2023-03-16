using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class PureFunctionReducerBinderTests
{
    [Test]
    public void ReducerBindi_WhenConstructedFromFuncAndGivenCorrectType_ShouldCallWithExpectedPayloadValue()
    {
        var reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(DummyPureFunctionReducer.Reduce);

        var res = reducer.ReducerBindingInfo();

        StringAssert.AreEqualIgnoringCase(nameof(DummyPureFunctionReducer.Reduce), res.Name);
        StringAssert.AreEqualIgnoringCase(nameof(DummyPureFunctionReducer), res.DeclaringType.Name);
    }
}