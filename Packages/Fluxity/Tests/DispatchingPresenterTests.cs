using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

public class DispatchingPresenterTests
{
    private class DummyDispatchingPresenter : DispatchingPresenter
    {
        public override void CreateBindings()
        {
        }

        public override void Display()
        {
        }
    }

    [Test]
    public void Dispatch_WhenNothingRegistered_ShouldNotThrow()
    {
        var go = new GameObject();
        var dispatchingPresenter = go.AddComponent<DummyDispatchingPresenter>();
        var storeSub = Substitute.For<IStore>();
        var dispatcher = new Dispatcher(storeSub);
        dispatchingPresenter.Inject(dispatcher);
        var command = new DummyCommand() { payload = 1 };

        dispatchingPresenter.Dispatch(command);

        storeSub.Received().ProcessCommand(Arg.Is<DummyCommand>(x => x == command));
    }
}
