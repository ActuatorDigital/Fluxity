using AIR.Fluxity.Tests.DummyTypes;
using NUnit.Framework;

public class DummyEffectTests
{
    private DummyEffect effect;

    [SetUp]
    public void SetUp()
    {
        effect = new DummyEffect();
    }

    [Test]
    public void DoEffect_WhenGivenCorrectType_ShouldCallWithExpectedPayloadValue()
    {
        var payloadVal = 3;
        var correctCommandType = new DummyCommand() { Payload = payloadVal };

        effect.DoEffect(correctCommandType, default);

        Assert.AreEqual(payloadVal, effect.accumPayload);
    }
}