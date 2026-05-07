using System.Collections;
using AIR.Flume;
using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FlumeIntegrationUnityTests
{
    private GameObject _rootGameObject;

    private DummyFlumePresenter _presenter;

    private class DummyFlumePresenter : MonoBehaviour
    {
        private FeatureObserver<DummyState> _dummyState = new();

        public string TextContent { get; private set; }

        public void Start()
        {
            _dummyState.OnStateChanged += Display;
        }

        public void OnDestroy()
        {
            _dummyState.OnStateChanged -= Display;
        }

        private void Display(DummyState state)
        {
            TextContent = state.value.ToString();
        }
    }

    public class DummyFluxityInitializer : FluxityInitializer
    {
        public override void RegisterFluxity(FluxityRegisterContext context)
        {
            context
                .Feature(new DummyState(), DummyReducers.RegisterAll)
                ;
        }

        protected override void RegisterServices(FlumeServiceContainer container)
        {
            container
                .Register<IDummyService, DummyService>()
            ;
        }
    }

    public interface IDummyService
    {
        int LastSignal { get; set; }
    }

    public class DummyService : IDummyService
    {
        public int LastSignal { get; set; }
    }

    public class DummyServiceHandle : Dependent
    {
        public IDummyService DummyService { get; private set; }

        public void Inject(IDummyService dummyService) => DummyService = dummyService;
    }

    [SetUp]
    public void SetUp()
    {
        _rootGameObject = new GameObject(nameof(FlumeIntegrationUnityTests));
        _rootGameObject.AddComponent<DummyFluxityInitializer>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_rootGameObject);
    }

    [UnityTest]
    public IEnumerator Display_WhenBoundAndUnityStartedAndMatchingCommandDispatched_ShouldOnlyCallDisplayTwice()
    {
        const int PAYLOAD = 5;
        var EXPECTED = PAYLOAD.ToString();
        _presenter = _rootGameObject.AddComponent<DummyFlumePresenter>();
        yield return null; //< give frame so start can be called

        new DispatcherHandle().Dispatch(new DummyCommand() { Payload = PAYLOAD });
        var result = _presenter.TextContent;

        Assert.AreEqual(EXPECTED, result);
    }
}