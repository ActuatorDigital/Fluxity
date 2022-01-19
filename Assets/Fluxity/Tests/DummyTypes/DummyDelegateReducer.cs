using System;

namespace AIR.Fluxity.Tests.DummyTypes
{
    internal class DummyDelegateReducer : Reducer<DummyState, DummyCommand>
    {
        private Action _customAction;

        public DummyDelegateReducer(Action customAction)
        {
            _customAction = customAction;
        }

        public override DummyState Reduce(DummyState state, DummyCommand command)
        {
            _customAction?.Invoke();
            return state;
        }
    }
}
