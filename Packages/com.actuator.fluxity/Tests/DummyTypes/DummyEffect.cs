using System;

namespace Actuator.Fluxity.Tests.DummyTypes
{
    internal class DummyEffect
    {
        public int accumPayload;
        public Action Action;
        public int callCount;

        public void DoEffect(DummyCommand command, IDispatcher dispatcher)
        {
            accumPayload += command.Payload;
            Action?.Invoke();
            callCount++;
        }
    }
}