using System;
using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class FluxityRegisterContextTests
{
    [Test]
    public void ctor_WhenNull_ShouldNotThrow()
    {
        static void Act() => new FluxityRegisterContext(null, null);

        Assert.DoesNotThrow(Act);
    }

    [Test]
    public void Feature_WhenNullStoreAndDispatcher_ShouldThrow()
    {
        var context = new FluxityRegisterContext(null, null);

        void Act() => context.Feature(default(DummyState));

        Assert.Throws<NullReferenceException>(Act);
    }

    [Test]
    public void Feature_WhenValid_ShouldReturnContext()
    {
        var store = new Store();
        var disp = new Dispatcher(store);
        var context = new FluxityRegisterContext(store, disp);

        var result = context.Feature(default(DummyState));

        Assert.IsNotNull(result);
    }

    [Test]
    public void Feature_WhenValid_ShouldHave1Feature()
    {
        var store = new Store();
        var disp = new Dispatcher(store);
        var context = new FluxityRegisterContext(store, disp);

        context.Feature(default(DummyState));

        Assert.AreEqual(1, store.GetAllFeatures().Count);
    }

    [Test]
    public void Hooks_WhenBound_ShouldInvoke()
    {
        var magicNumber = 123;
        var pre = 0;
        var post = 0;
        var store = new Store();
        var disp = new Dispatcher(store);
        var context = new FluxityRegisterContext(store, disp);
        context
            .OnPreDispatch<DummyCommand>(cmd => pre = cmd.Payload)
            .OnPostDispatch<DummyCommand>(cmd => post = cmd.Payload)
            ;

        disp.Dispatch(new DummyCommand { Payload = magicNumber });

        Assert.AreEqual(magicNumber, pre);
        Assert.AreEqual(magicNumber, post);
    }
}
