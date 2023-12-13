using System;

namespace AIR.Fluxity.Tests.DummyTypes
{
    public class DummyDelegatePresenter : Presenter
    {
        public FeatureBinding<DummyState> DummyStatePresenterBinding { get; set; }
        public Action<DummyState> OnDisplay { get; set; }

        public override void CreateBindings()
        {
        }

        public override void Display()
        {
            var state = DummyStatePresenterBinding.State;
            OnDisplay?.Invoke(state);
        }
    }
}