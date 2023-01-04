using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NSubstitute;
using NUnit.Framework;

public class FeaturePresenterBindingTests
{
    private IFeature<DummyState> _feature;

    [SetUp]
    public void SetUp()
    {
        _feature = new Feature<DummyState>(default);
    }

    [Test]
    public void CurrentState_WhenFeatureStateUpdates_ShouldInvoke()
    {
        const int EXPECTED = 5;
        using (var featurePresenterBinding = new FeaturePresenterBinding<DummyState>(Substitute.For<IPresenter>()))
        {
            featurePresenterBinding.Inject(_feature);

            _feature.SetState(new DummyState() { value = EXPECTED});

            Assert.AreEqual(EXPECTED, featurePresenterBinding.CurrentState.value);
        }
    }

    [Test]
    public void Display_WhenFeatureStateUpdates_ShouldInvoke()
    {
        var presenterSubstitute = Substitute.For<IPresenter>();
        using (var featurePresenterBinding = new FeaturePresenterBinding<DummyState>(presenterSubstitute))
        {
            featurePresenterBinding.Inject(_feature);

            _feature.SetState(new DummyState());

            presenterSubstitute.Received().Display();
        }
    }

    [Test]
    public void Display_WhenDisposed_ShouldNotInvoke()
    {
        var presenterSubstitute = Substitute.For<IPresenter>();
        using (var featurePresenterBinding = new FeaturePresenterBinding<DummyState>(presenterSubstitute))
        {
            featurePresenterBinding.Inject(_feature);
        }

        _feature.SetState(new DummyState());

        presenterSubstitute.DidNotReceive().Display();
    }
}
