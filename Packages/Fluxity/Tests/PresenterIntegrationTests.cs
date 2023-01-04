using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;
using UnityEngine;

public class PresenterIntegrationTests
{
    private GameObject _rootGameObject;
    private Store _store;
    private Dispatcher _dispatcher;
    private Feature<DummyState> _feature;
    private DummyDelegatePresenter _dummyPresenter;


    [SetUp]
    public void SetUp()
    {
        _store = new Store();
        _dispatcher = new Dispatcher();
        _feature = new Feature<DummyState>(default);
        _rootGameObject = new GameObject(nameof(PresenterIntegrationTests));

        _dispatcher.Inject(_store);
        _feature.Inject(_store);
        _store.CreateAndRegister<DummyState, DummyCommand>(DummyPureFunctionReducer.Reduce);

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
    public void Display_WhenNotBoundToStateAndStateChanged_ShouldNotCallDisplay()
    {
        const int INITIAL = 2;
        const int EXPECTED = 1;
        var result = INITIAL;
        _dummyPresenter.OnDisplay = (x) => result = EXPECTED;

        _feature.SetState(new DummyState());

        Assert.AreNotEqual(EXPECTED, result);
    }

    [Test]
    public void Display_WhenBoundAndMatchingStateChanged_ShouldInvokeDelegate()
    {
        const int INITIAL = 2;
        const int EXPECTED = 1;
        var result = INITIAL;
        var statePresenterBinding = _dummyPresenter.Bind<DummyState>();
        statePresenterBinding.Inject(_feature);
        _dummyPresenter.DummyStatePresenterBinding = statePresenterBinding;
        _dummyPresenter.OnDisplay = (x) => result = EXPECTED;

        _feature.SetState(new DummyState());

        Assert.AreEqual(EXPECTED, result);
    }

    [Test]
    public void Display_WhenNotBoundToStateAndCommandDispatched_ShouldNotCallDisplay()
    {
        const int INITIAL = 2;
        const int EXPECTED = 1;
        var result = INITIAL;
        _dummyPresenter.OnDisplay = (x) => result = EXPECTED;

        _dispatcher.Dispatch(new DummyCommand());

        Assert.AreNotEqual(EXPECTED, result);
    }

    [Test]
    public void Display_WhenBoundAndMatchingCommandDispatched_ShouldInvokeDelegate()
    {
        const int INITIAL = 2;
        const int EXPECTED = 1;
        var result = INITIAL;
        var statePresenterBinding = _dummyPresenter.Bind<DummyState>();
        statePresenterBinding.Inject(_feature);
        _dummyPresenter.DummyStatePresenterBinding = statePresenterBinding;
        _dummyPresenter.OnDisplay = (x) => result = EXPECTED;

        _dispatcher.Dispatch(new DummyCommand());

        Assert.AreEqual(EXPECTED, result);
    }
}
