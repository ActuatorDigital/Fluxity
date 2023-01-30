using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

public class StoreTests
{
    private Store _store;

    [SetUp]
    public void SetUp()
    {
        _store = new Store();
    }

    [Test]
    public void ProcessCommand_WhenNoFeaturesRegistered_ShouldNotThrow()
    {
        void Act() => _store.ProcessCommand(new DummyCommand());

        Assert.DoesNotThrow(Act);
    }

    [Test]
    public void ProcessCommand_WhenNoMatchingFeaturesRegistered_ShouldNotThrow()
    {
        var featureSubstitute = Substitute.For<IFeature<DummyState>>();
        featureSubstitute.GetStateType.Returns(typeof(DummyState));
        _store.AddFeature(featureSubstitute);

        void Act() => _store.ProcessCommand(new DummyCommand());

        Assert.DoesNotThrow(Act);
    }

    [Test]
    public void RegisterReducer_WhenNoMatchingFeaturesRegistered_ShouldThrow()
    {
        var substituteReducer = Substitute.For<IReducer<DummyState, DummyCommand>>();
        substituteReducer.GetStateType.Returns(typeof(DummyState));

        void Act() => _store.RegisterReducer(substituteReducer);

        Assert.Throws<KeyNotFoundException>(Act);
    }

    [Test]
    public void GetAllFeatures_When2Registered_ShouldReturnBothItems()
    {
        var featureSubstitute = Substitute.For<IFeature<DummyState>>();
        featureSubstitute.GetStateType.Returns(typeof(DummyState));
        _store.AddFeature(featureSubstitute);
        var featureSubstitute2 = Substitute.For<IFeature<OtherDummyState>>();
        featureSubstitute2.GetStateType.Returns(typeof(OtherDummyState));
        _store.AddFeature(featureSubstitute2);
        var substituteReducer = Substitute.For<IReducer<DummyState, DummyCommand>>();
        substituteReducer.GetStateType.Returns(typeof(DummyState));

        var res = _store.GetAllFeatures();

        Assert.IsNotNull(res);
        CollectionAssert.IsNotEmpty(res);
        Assert.AreEqual(2, res.Count);
        CollectionAssert.Contains(res, featureSubstitute);
        CollectionAssert.Contains(res, featureSubstitute2);
    }
}
