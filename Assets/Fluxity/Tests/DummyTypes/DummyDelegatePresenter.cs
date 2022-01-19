using System;

namespace AIR.Fluxity.Tests.DummyTypes
{
    public class DummyDelegatePresenter : Presenter
    {
        public IStatePresenterBinding<DummyState> DummyStatePresenterBinding { get; set; }
        public Action<DummyState> OnDisplay { get; set; }

        public override void CreateBindings()
        {
        }

        public override void Display()
        {
            var state = DummyStatePresenterBinding.CurrentState;
            OnDisplay?.Invoke(state);
        }
    }
}