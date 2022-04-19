using AIR.Fluxity;
using UnityEngine;

namespace Examples.Spinner
{
    public class SpinningObjectPresenter : Presenter
    {
        [SerializeField] private SpinnerView uSpinnerView;
        private IFeaturePresenterBinding<SpinState> _spinStateBinding;

        public override void CreateBindings()
        {
            _spinStateBinding = Bind<SpinState>();
        }

        public override void Display()
        {
            var currentState = _spinStateBinding.CurrentState;
            uSpinnerView.SetSpinRate(currentState.DegreesPerSecond);
            if (currentState.DoSpin)
                uSpinnerView.StartSpin();
            else
                uSpinnerView.StopSpin();
        }
    }
}