using System;

namespace AIR.Fluxity.Tests.DummyTypes
{
    public class DummyPresenter : Presenter
    {
        public FeatureBinding<DummyState> DummyStatePresenterBinding { get; set; }

        public int DisplayCallCount { get; private set; }
        public IStore Store { get; set; }
        public DummyState StateAtLastDisplay { get; private set; }

        public override void CreateBindings()
        {
            DummyStatePresenterBinding = Bind<DummyState>();
            //Only needed during test to remove need for test to use auto DI
            DummyStatePresenterBinding.Inject(Store);
        }

        public override void Display()
        {
            DisplayCallCount++;
            StateAtLastDisplay = DummyStatePresenterBinding.State;
        }
    }
}