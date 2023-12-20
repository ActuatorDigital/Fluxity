using AIR.Fluxity;
using UnityEngine;

namespace Examples.Spinner
{
    public class SpinningObjectPresenter : MonoBehaviour
    {
        [SerializeField] private SpinnerView uSpinnerView;
        private readonly FeatureObserver<SpinState> _spinState = new();

        public void Start()
        {
            _spinState.OnStateChanged += Display;
        }

        public void OnDestroy()
        {
            _spinState.OnStateChanged -= Display;
            _spinState.Dispose();
        }

        private void Display(SpinState state)
        {
            uSpinnerView.SetSpinRate(state.DegreesPerSecond);
            if (state.DoSpin)
                uSpinnerView.StartSpin();
            else
                uSpinnerView.StopSpin();
        }
    }
}