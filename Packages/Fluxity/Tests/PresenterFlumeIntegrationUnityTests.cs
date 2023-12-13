using System.Collections;
using AIR.Flume;
using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PresenterFlumeIntegrationUnityTests
{
    private GameObject _rootGameObject;

    private DummyFlumePresenter _presenter;
    private DispatcherHandle _dipatcherHandle;

    private class DummyFlumePresenter : Presenter
    {
        private FeatureBinding<DummyState> _dummyState;

        public string TextContent { get; private set; }

        public override void CreateBindings()
        {
            _dummyState = Bind<DummyState>();
        }

        public override void Display()
        {
            TextContent = _dummyState.State.value.ToString();
        }
    }

    public class DummyFlumeServiceInstaller : ServiceInstaller
    {
        protected override void InstallServices(FlumeServiceContainer container)
        {
            container
                .RegisterFluxity(DummyFluxityInitializer.Setup)

                .Register<IDummyService, DummyService>()
                ;
        }
    }

    public class DummyFluxityInitializer : FluxityInitializer
    {
        public static void Setup(FluxityFlumeRegisterContext context)
        {
            context
                .Feature(new DummyState())
                    .Reducer<DummyCommand>(DummyPureFunctionReducer.Reduce)
                ;
        }

        protected override void Initialize()
        {
            var dummyCommandEffect = new DummyCommandEffect();
            CreateEffect<DummyCommand>(dummyCommandEffect.DoEffect);
        }
    }

    private class DispatcherHandle : Dependent
    {
        private IDispatcher _dispatcher;

        public void Inject(IDispatcher dispatcher) => _dispatcher = dispatcher;

        public void Dispatch<TCommand>(TCommand command)
             where TCommand : ICommand
        {
            _dispatcher.Dispatch(command);
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
        public IDummyService DummySerivce { get; private set; }

        public void Inject(IDummyService dummyService) => DummySerivce = dummyService;
    }

    public class DummyCommandEffect : Dependent
    {
        private IDummyService _dummyService;

        public void Inject(IDummyService dummyService) => _dummyService = dummyService;

        public void DoEffect(DummyCommand command, IDispatcher _)
        {
            _dummyService.LastSignal = command.payload;
        }
    }

    [SetUp]
    public void SetUp()
    {
        _rootGameObject = new GameObject(nameof(PresenterFlumeIntegrationUnityTests));
        _rootGameObject.AddComponent<DummyFlumeServiceInstaller>();
        _rootGameObject.AddComponent<DummyFluxityInitializer>();

        _dipatcherHandle = new DispatcherHandle();
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

        _dipatcherHandle.Dispatch(new DummyCommand() { payload = PAYLOAD });
        var result = _presenter.TextContent;

        Assert.AreEqual(EXPECTED, result);
    }

    [UnityTest]
    public IEnumerator Effect_WhenBoundAndUnityStartedAndMatchingCommandDispatched_ShouldPutExpectedValueInInjectedService()
    {
        const int EXPECTED = 5;
        var dummyServiceHandle = new DummyServiceHandle();
        yield return null; //< give frame so start can be called

        _dipatcherHandle.Dispatch(new DummyCommand() { payload = EXPECTED });
        var result = dummyServiceHandle.DummySerivce.LastSignal;

        Assert.AreEqual(EXPECTED, result);
    }
}