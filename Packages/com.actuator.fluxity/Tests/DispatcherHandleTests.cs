using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NSubstitute;
using NUnit.Framework;

public class DispatcherHandleTests
{
    [Test]
    public void Dispatch_WhenCalled_ShouldCauseDispatcherToDispatch()
    {
        // Arrange
        var dispatcher = Substitute.For<IDispatcher>();
        var dispatcherHandle = new DispatcherHandle();
        dispatcherHandle.Inject(dispatcher);

        // Act
        dispatcherHandle.Dispatch(Substitute.For<ICommand>());

        // Assert
        dispatcher.Received().Dispatch(Arg.Any<ICommand>());
    }

    [Test]
    public void Dispatch_WhenCalledWithCommand_ShouldHaveDispatcherDispatchSameCommand()
    {
        // Arrange
        const int PAYLOAD_VALUE = 1;
        var dispatcher = Substitute.For<IDispatcher>();
        var dispatcherHandle = new DispatcherHandle();
        var command = new DummyCommand { Payload = PAYLOAD_VALUE };
        dispatcherHandle.Inject(dispatcher);

        // Act
        dispatcherHandle.Dispatch(command);

        // Assert
        dispatcher.Received().Dispatch(Arg.Is<DummyCommand>(x => x == command));
    }
}