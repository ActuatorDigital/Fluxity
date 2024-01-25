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
    public void Effect_WhenValid_ShouldHave1Effect()
    {
        var store = new Store();
        var disp = new Dispatcher(store);
        var context = new FluxityRegisterContext(store, disp);

        context.Effect<DummyCommand>((cmd, disp) => { });

        Assert.AreEqual(1, disp.GetAllEffectCommandTypes().Count);
    }
}
