using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;
using System;
using UnityEngine;

public class PresenterTests
{
    private Store _store;
    private Feature<DummyState> _feature;
    private GameObject _rootGameObject;
    private DummyPresenter _dummyPresenter;

    [SetUp]
    public void SetUp()
    {
        _store = new Store();
        _feature = new Feature<DummyState>(default);
        _store.AddFeature(_feature);
        _rootGameObject = new GameObject(nameof(PresenterTests));
        var presenterGO = new GameObject("PresenterNoDi");
        presenterGO.transform.SetParent(_rootGameObject.transform);
        _dummyPresenter = presenterGO.AddComponent<DummyPresenter>();
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.DestroyImmediate(_rootGameObject);
    }

    [Test]
    public void Display_WhenNoFeatureBoundToPresenter_ShouldThrow()
    {
        var statePresenterBinding = _dummyPresenter.Bind<DummyState>();
        _dummyPresenter.DummyStatePresenterBinding = statePresenterBinding;

        void Act() => _dummyPresenter.Display();

        Assert.Throws<NullReferenceException>(Act);
    }

    [Test]
    public void Display_WhenSetupCorrectly_ShouldDisplayAndHaveExpectedState()
    {
        var featureValue = -1;
        _feature.SetState(new DummyState() { value = featureValue });
        var statePresenterBinding = _dummyPresenter.Bind<DummyState>();
        (statePresenterBinding as FeatureObserver<DummyState>).Inject(_store);
        _dummyPresenter.DummyStatePresenterBinding = statePresenterBinding;

        _dummyPresenter.Display();

        Assert.AreEqual(1, _dummyPresenter.DisplayCallCount);
        Assert.AreEqual(featureValue, _dummyPresenter.StateAtLastDisplay.value);
    }
}
