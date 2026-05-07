using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class FluxityRegisterEffectContextTests
{
    [Test]
    public void Method_WhenValid_ShouldHave1Effect()
    {
        var store = new Store();
        var disp = new Dispatcher(store);
        var context = new FluxityRegisterContext(store, disp);

        context.Effect(new DummyEffect())
            .Method<DummyCommand>(x => x.DoEffect);

        Assert.AreEqual(1, disp.GetAllEffectCommandTypes().Count);
    }
}