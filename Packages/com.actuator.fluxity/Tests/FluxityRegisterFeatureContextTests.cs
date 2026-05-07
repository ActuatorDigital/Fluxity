using System.Linq;
using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class FluxityRegisterFeatureContextTests
{
    [Test]
    public void Feature_WhenValid_ShouldHave1Feature()
    {
        var store = new Store();
        var disp = new Dispatcher(store);
        var context = new FluxityRegisterContext(store, disp);

        context.Feature(default(DummyState))
            .Reducer<DummyCommand>(DummyReducers.Reduce);

        Assert.AreEqual(1, store.GetAllFeatures().First().GetAllHandledCommandTypes().Count);
    }
}
