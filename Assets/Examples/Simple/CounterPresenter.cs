using AIR.Fluxity;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.Simple
{
    public class CounterPresenter : MonoBehaviour
    {
        [SerializeField] private Text uLabelText;
        [SerializeField] private Text uCountText;

        private readonly FeatureObserver<CounterState> _counterState = new();

        public void Start()
        {
            _counterState.OnStateChanged += Display;
            uLabelText.text = "Current Count:";
        }

        public void OnDestroy()
        {
            _counterState.OnStateChanged -= Display;
            _counterState.Dispose();
        }

        private void Display(CounterState state)
        {
            uCountText.text = state.CurrentCount.ToString();
        }
    }
}