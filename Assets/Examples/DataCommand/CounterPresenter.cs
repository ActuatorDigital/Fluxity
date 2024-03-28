using AIR.Fluxity;
using UnityEngine;

namespace Examples.DataCommand
{
    public class CounterPresenter : MonoBehaviour
    {
        [SerializeField] private CounterView _counterView;

        private readonly FeatureObserver<CounterState> _counterState = new();

        public void Start()
        {
            _counterState.OnStateChanged += Display;
        }

        public void OnDestroy()
        {
            _counterState.OnStateChanged -= Display;
            _counterState.Dispose();
        }

        private void Display(CounterState state)
        {
            _counterView.UpdateModel(new CounterView.Model { CurrentCount = state.CurrentCount });
        }
    }
}