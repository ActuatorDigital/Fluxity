using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class FeatureObserverAggregateTests
{
    private IStore _store;
    private Feature<DummyState> _feature;
    private Feature<OtherDummyState> _otherFeature;

    [SetUp]
    public void SetUp()
    {
        _store = new Store();
        _feature = new Feature<DummyState>(default);
        _otherFeature = new Feature<OtherDummyState>(default);
        _store.AddFeature(_feature);
        _store.AddFeature(_otherFeature);
    }

    [Test]
    public void Empty_WhenCreateAndDispose_ShouldNotThrow()
    {
        using var featurePresenterBinding = new FeatureObserverAggregate();
    }

    [Test]
    public void OnAnyStateChanged_WhenSetState_ShouldInvoke1()
    {
        var changeCount = 0;
        using var featurePresenterBinding = new FeatureObserverAggregate();
        featurePresenterBinding.Inject(_store);
        featurePresenterBinding.Bind<DummyState>();
        featurePresenterBinding.OnAnyStateChanged += () => { changeCount++; };

        _feature.SetState(new DummyState());

        Assert.AreEqual(1, changeCount);
    }

    [Test]
    public void OnAnyStateChanged_WhenMultipleBoundAndSetStateOnAAndThenB_ShouldInvoke2()
    {
        var changeCount = 0;
        using var featurePresenterBinding = new FeatureObserverAggregate();
        featurePresenterBinding.Inject(_store);
        featurePresenterBinding.Bind<DummyState>();
        featurePresenterBinding.Bind<OtherDummyState>();
        featurePresenterBinding.OnAnyStateChanged += () => { changeCount++; };

        _feature.SetState(new DummyState());
        _otherFeature.SetState(new OtherDummyState());

        Assert.AreEqual(2, changeCount);
    }

    [Test]
    public void OnAnyStateChanged_WhenMultipleBoundAndThenDisposeAndThenSetStateOnAAndThenB_ShouldNotInvoke()
    {
        var changeCount = 0;
        using (var featurePresenterBinding = new FeatureObserverAggregate())
        {

            featurePresenterBinding.Inject(_store);
            featurePresenterBinding.Bind<DummyState>();
            featurePresenterBinding.Bind<OtherDummyState>();
            featurePresenterBinding.OnAnyStateChanged += () => { changeCount++; };
        }

        _feature.SetState(new DummyState());
        _otherFeature.SetState(new OtherDummyState());

        Assert.AreEqual(0, changeCount);
    }
}
