using AIR.Fluxity;
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
}