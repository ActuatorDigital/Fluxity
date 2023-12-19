using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class PureFunctionReducerBinderTests
{
    [Test]
    public void ReducerBindingInfo_WhenConstructedFromFuncAndGivenCorrectType_ShouldCallWithExpectedPayloadValue()
    {
        var reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(DummyReducers.Reduce);

        var res = reducer.ReducerBindingInfo();

        StringAssert.AreEqualIgnoringCase(nameof(DummyReducers.Reduce), res.Name);
        StringAssert.AreEqualIgnoringCase(nameof(DummyReducers), res.DeclaringType.Name);
    }
}