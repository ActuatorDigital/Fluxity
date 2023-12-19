using AIR.Fluxity;
using UnityEngine;

namespace Examples.Spinner
{
    public class SpinningObjectPresenter : Presenter
    {
        [SerializeField] private SpinnerView uSpinnerView;
        private IFeatureView<SpinState> _spinStateBinding;

        public override void CreateBindings()
        {
            _spinStateBinding = Bind<SpinState>();
        }

        public override void Display()
        {
            var state = _spinStateBinding.State;
            uSpinnerView.SetSpinRate(state.DegreesPerSecond);
            if (state.DoSpin)
                uSpinnerView.StartSpin();
            else
                uSpinnerView.StopSpin();
        }
    }
}