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
        var dispatcher = new Dispatcher();
        var storeSub = Substitute.For<IStore>();
        dispatchingPresenter.Inject(dispatcher);
        dispatcher.Inject(storeSub);
        var command = new DummyCommand() { payload = 1 };

        dispatchingPresenter.Dispatch(command);

        storeSub.Received().ProcessCommand(Arg.Is<DummyCommand>(x => x == command));
    }
}
