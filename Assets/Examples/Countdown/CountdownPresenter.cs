using AIR.Fluxity;
using Examples.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.Countdown
{
    public class CountdownPresenter : MonoBehaviour
    {
        [SerializeField] private Text uCountdownDisplayText;
        [SerializeField] private ButtonView uStartCountdownButton;
        [SerializeField] private ButtonView uStopCountdownButton;
        [SerializeField] private float uCountdownSeconds;

        private float _secondsRemaining = 0;
        private bool _isRunning = false;
        private FeatureObserver<CountdownState> _countdownState = new();

        public void Start()
        {
            uStartCountdownButton.SetButtonText($"Start ({uCountdownSeconds}s)");
            uStartCountdownButton.SetOnClickedCallback(OnClickStart);
            uStopCountdownButton.SetButtonText("Stop");
            uStopCountdownButton.SetOnClickedCallback(OnClickStop);
            _countdownState.OnStateChanged += Display;
        }

        public void OnDestroy()
        {
            _countdownState.OnStateChanged -= Display;
        }

        private void Display(CountdownState state)
        {
            if (!state.IsRunning)
            {
                uCountdownDisplayText.text = "Countdown Disabled";
                _isRunning = false;
            }
            else
            {
                _secondsRemaining = state.CountdownDurationSeconds;
                _isRunning = true;
            }
        }

        // NOTE: Update loop used instead of coroutine for example simplicity.
        public void Update()
        {
            if (!_isRunning)
                return;

            _secondsRemaining -= Time.deltaTime;
            uCountdownDisplayText.text = $"{_secondsRemaining:F2} s remaining";
            if (_secondsRemaining <= 0)
                DispatchStop();
        }

        private void OnClickStart()
            => new DispatcherHandle().Dispatch(new StartCountdownCommand { Seconds = uCountdownSeconds });

        private void OnClickStop()
            => DispatchStop();

        private void DispatchStop()
        {
            new DispatcherHandle().Dispatch(new StopCountdownCommand());
        }
    }
}