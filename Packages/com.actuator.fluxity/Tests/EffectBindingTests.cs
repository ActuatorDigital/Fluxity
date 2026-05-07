using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class EffectBindingTests
{
    [Test]
    public void Dispatch_WhenEffectRegistered_ShouldReturn1()
    {
        var dummyEffect = new DummyEffect();
        var effect = new EffectBinding<DummyCommand>(dummyEffect.DoEffect);

        var res = effect.EffectBindingInfo();

        StringAssert.AreEqualIgnoringCase(nameof(DummyEffect.DoEffect), res.Name);
        StringAssert.AreEqualIgnoringCase(nameof(DummyEffect), res.DeclaringType.Name);
    }
}