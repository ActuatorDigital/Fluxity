using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class FeatureHandleTests
{
    private IStore _store;
    private Feature<DummyState> _feature;

    [SetUp]
    public void SetUp()
    {
        _store = new Store();
        _feature = new Feature<DummyState>(default);
        _store.AddFeature(_feature);
    }

    [Test]
    public void State_WhenFeatureStateUpdates_ShouldHaveExpectedValue()
    {
        const int EXPECTED = 5;
        var handle = new FeatureHandle<DummyState>();
        handle.Inject(_store);

        _feature.SetState(new DummyState { value = EXPECTED });
        Assert.AreEqual(EXPECTED, handle.State.value);
    }
}