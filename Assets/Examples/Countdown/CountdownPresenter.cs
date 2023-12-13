using AIR.Fluxity;
using Examples.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.Countdown
{
    public class CountdownPresenter : Presenter
    {
        [SerializeField] private Text uCountdownDisplayText;
        [SerializeField] private ButtonView uStartCountdownButton;
        [SerializeField] private ButtonView uStopCountdownButton;
        [SerializeField] private float uCountdownSeconds;

        private float _secondsRemaining = 0;
        private bool _isRunning = false;
        private FeatureBinding<CountdownState> _countdownStateBinding;

        public override void Display()
        {
            // State is source of truth and should always override local values.
            var currentState = _countdownStateBinding.State;
            if (!currentState.IsRunning)
            {
                uCountdownDisplayText.text = "Countdown Disabled";
                _isRunning = false;
            }
            else
            {
                _secondsRemaining = currentState.CountdownDurationSeconds;
                _isRunning = true;
            }
        }

        public override void CreateBindings()
        {
            _countdownStateBinding = Bind<CountdownState>();
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

        protected override void SetUp()
        {
            uStartCountdownButton.SetButtonText($"Start ({uCountdownSeconds}s)");
            uStartCountdownButton.SetOnClickedCallback(OnClickStart);
            uStopCountdownButton.SetButtonText("Stop");
            uStopCountdownButton.SetOnClickedCallback(OnClickStop);
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