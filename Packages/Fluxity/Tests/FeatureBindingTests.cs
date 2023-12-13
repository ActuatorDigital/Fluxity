using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NSubstitute;
using NUnit.Framework;

public class FeatureBindingTests
{
    private IStore _store;
    private IFeature<DummyState> _feature;

    [SetUp]
    public void SetUp()
    {
        _store = new Store();
        _feature = new Feature<DummyState>(default);
        _store.AddFeature(_feature);
    }

    [Test]
    public void CurrentState_WhenFeatureStateUpdates_ShouldHaveExpectedValue()
    {
        const int EXPECTED = 5;
        using (var featurePresenterBinding = new FeatureBinding<DummyState>())
        {
            featurePresenterBinding.Inject(_store);

            _feature.SetState(new DummyState() { value = EXPECTED});

            Assert.AreEqual(EXPECTED, featurePresenterBinding.State.value);
        }
    }

    [Test]
    public void Display_WhenFeatureStateUpdates_ShouldInvoke()
    {
        var invoked = false;
        using (var featurePresenterBinding = new FeatureBinding<DummyState>())
        {
            featurePresenterBinding.Inject(_store);
            featurePresenterBinding.OnStateChanged += x => invoked = true;

            _feature.SetState(new DummyState());

            Assert.IsTrue(invoked);
        }
    }

    [Test]
    public void Display_WhenDisposed_ShouldNotInvoke()
    {
        var invoked = false;
        using (var featurePresenterBinding = new FeatureBinding<DummyState>())
        {
            featurePresenterBinding.Inject(_store);
            featurePresenterBinding.OnStateChanged += x => invoked = true;
        }

        _feature.SetState(new DummyState());

        Assert.IsFalse(invoked);
    }
}
