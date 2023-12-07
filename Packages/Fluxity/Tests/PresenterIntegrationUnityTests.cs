using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class PresenterIntegrationUnityTests
{
    private GameObject _rootGameObject;
    private Store _store;
    private Dispatcher _dispatcher;
    private Feature<DummyState> _feature;
    private CustomUnityTestDummyDelegatePresenter _presenter;

    private class CustomUnityTestDummyDelegatePresenter : DummyDelegatePresenter
    {
        public int DisplayCallCount { get; private set; }
        public IStore Store { get; set; }

        public override void CreateBindings()
        {
            DummyStatePresenterBinding = Bind<DummyState>();
            //Only needed during test to remove need for test to use auto DI
            DummyStatePresenterBinding.Inject(Store);
        }

        public override void Display()
        {
            base.Display();
            DisplayCallCount++;
        }
    }

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _store = new Store();
        _dispatcher = new Dispatcher(_store);
        _feature = new Feature<DummyState>(default);
        _rootGameObject = new GameObject(nameof(PresenterIntegrationUnityTests));
        _store.AddFeature(_feature); 
        var reducer = new PureFunctionReducerBinder<DummyState, DummyCommand>(DummyPureFunctionReducer.Reduce);
        _feature.Register(reducer);

        var presenterGO = new GameObject("PresenterNoDi");
        presenterGO.transform.SetParent(_rootGameObject.transform);
        _presenter = presenterGO.AddComponent<CustomUnityTestDummyDelegatePresenter>();
        _presenter.Store = _store;
        yield return null;  //give frame so start can be called
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_rootGameObject);
    }

    [UnityTest]
    public IEnumerator Display_WhenBoundAndUnityStarted_ShouldHaveCallDisplayOnce()
    {
        const int EXPECTED = 1;
        var result = 0;

        result = _presenter.DisplayCallCount;
        
        Assert.AreEqual(EXPECTED, result);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Display_WhenBoundAndUnityStartedAndNonMatchingCommandDispatched_ShouldOnlyCallDisplayOnce()
    {
        const int EXPECTED = 1;
        var result = 0;

        _dispatcher.Dispatch(new OtherDummyCommand());
        result = _presenter.DisplayCallCount;

        Assert.AreEqual(EXPECTED, result);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Display_WhenBoundAndUnityStartedAndMatchingCommandDispatched_ShouldOnlyCallDisplayTwice()
    {
        const int EXPECTED = 2;
        var result = 0;

        _dispatcher.Dispatch(new DummyCommand());
        result = _presenter.DisplayCallCount;

        Assert.AreEqual(EXPECTED, result);
        yield return null;
    }
}