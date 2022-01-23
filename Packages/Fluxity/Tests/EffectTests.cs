using AIR.Fluxity;
using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class EffectTests
{
    private DummyEffect effect;

    [SetUp]
    public void SetUp()
    {
        effect = new DummyEffect(NSubstitute.Substitute.For<IDispatcher>());
    }

    [Test]
    public void DoEffect_WhenGivenIncorrectType_ShouldThrow()
    {
        var wrongCommandType = new OtherDummyCommand();

        void Act()
        {
            effect.DoEffect(wrongCommandType);
        }

        Assert.Throws<System.InvalidCastException>(Act);
    }

    [Test]
    public void DoEffect_WhenGivenCorrectType_ShouldCallWithExpectedPayloadValue()
    {
        var payloadVal = 3;
        var correctCommandType = new DummyCommand() { payload = payloadVal };

        effect.DoEffect(correctCommandType);

        Assert.AreEqual(payloadVal, effect.accumPayload);
    }
}