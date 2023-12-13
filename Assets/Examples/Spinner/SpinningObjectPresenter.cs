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
            var currentState = _spinStateBinding.State;
            uSpinnerView.SetSpinRate(currentState.DegreesPerSecond);
            if (currentState.DoSpin)
                uSpinnerView.StartSpin();
            else
                uSpinnerView.StopSpin();
        }
    }
}