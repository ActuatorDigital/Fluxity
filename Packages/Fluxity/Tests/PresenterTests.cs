using AIR.Flume;
using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class PresenterTests
{
    private Store _store;
    private Feature<DummyState> _feature;
    private GameObject _rootGameObject;
    private DummyDelegatePresenter _dummyPresenter;

    [SetUp]
    public void SetUp()
    {
        _store = new Store();
        _feature = new Feature<DummyState>(default);
        _store.AddFeature(_feature);
        _rootGameObject = new GameObject(nameof(PresenterTests));
        var presenterGO = new GameObject("PresenterNoDi");
        presenterGO.transform.SetParent(_rootGameObject.transform);
        _dummyPresenter = presenterGO.AddComponent<DummyDelegatePresenter>();
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
    public void Display_WhenSetupCorrectly_ShouldInvokeProvidedDelegate()
    {
        const int INITIAL = 2;
        const int EXPECTED = 1;
        var result = INITIAL;
        var statePresenterBinding = _dummyPresenter.Bind<DummyState>();
        statePresenterBinding.Inject(_store);
        _dummyPresenter.DummyStatePresenterBinding = statePresenterBinding;
        _dummyPresenter.OnDisplay = (x) => result = EXPECTED;

        _dummyPresenter.Display();

        Assert.AreEqual(EXPECTED, result);
    }
}
