using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;
using UnityEngine;

public class PresenterIntegrationTests
{
    private const int StartingDummyStateValue = -1;
    private GameObject _rootGameObject;
    private Store _store;
    private Dispatcher _dispatcher;
    private Feature<DummyState> _feature;
    private DummyPresenter _dummyPresenter;

    [SetUp]
    public void SetUp()
    {
        _store = new Store();
        _dispatcher = new Dispatcher(_store);
        _feature = new Feature<DummyState>(new DummyState { value = StartingDummyStateValue });
        _rootGameObject = new GameObject(nameof(PresenterIntegrationTests));
        _store.AddFeature(_feature);
        var reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(DummyReducers.Reduce);
        _feature.Register(reducer);

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
    public void Display_WhenNotBoundToStateAndStateChanged_ShouldNotCallDisplay()
    {
        _feature.SetState(new DummyState());

        Assert.AreEqual(0, _dummyPresenter.DisplayCallCount);
    }

    [Test]
    public void Display_WhenBoundAndMatchingStateChanged_ShouldHaveDisplayedAndLastState()
    {
        var expected = 10;
        var statePresenterBinding = _dummyPresenter.Bind<DummyState>();
        (statePresenterBinding as FeatureObserver<DummyState>).Inject(_store);
        _dummyPresenter.DummyStatePresenterBinding = statePresenterBinding;

        _feature.SetState(new DummyState() { value = expected });

        Assert.AreEqual(1, _dummyPresenter.DisplayCallCount);
        Assert.AreEqual(expected, _dummyPresenter.StateAtLastDisplay.value);
    }

    [Test]
    public void Display_WhenNotBoundToStateAndCommandDispatched_ShouldNotCallDisplay()
    {
        _dispatcher.Dispatch(new DummyCommand());

        Assert.AreEqual(0, _dummyPresenter.DisplayCallCount);
    }

    [Test]
    public void Display_WhenBoundAndMatchingCommandDispatched_ShouldHaveDisplayedAndLastState()
    {
        var statePresenterBinding = _dummyPresenter.Bind<DummyState>();
        (statePresenterBinding as FeatureObserver<DummyState>).Inject(_store);
        _dummyPresenter.DummyStatePresenterBinding = statePresenterBinding;

        _dispatcher.Dispatch(new DummyCommand());

        Assert.AreEqual(1, _dummyPresenter.DisplayCallCount);
        Assert.AreEqual(StartingDummyStateValue, _dummyPresenter.StateAtLastDisplay.value);
    }
}
