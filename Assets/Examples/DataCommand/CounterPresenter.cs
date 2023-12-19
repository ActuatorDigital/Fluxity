using AIR.Fluxity;
using Examples.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.DataCommand
{
    public class CounterPresenter : MonoBehaviour
    {
        [SerializeField] private Text uLabelText;
        [SerializeField] private Text uCountText;
        [SerializeField] private InputField uInputField;
        [SerializeField] private ButtonView uButtonView;

        private readonly FeatureObserver<CounterState> _counterState = new();

        public void Start()
        {
            _counterState.OnStateChanged += Display;
            SetUp();
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

        protected void SetUp()
        {
            uLabelText.text = "Current Count:";
            uButtonView.SetButtonText("Change Count");
            uButtonView.SetOnClickedCallback(OnButtonClick);
            uInputField.contentType = InputField.ContentType.IntegerNumber;
            uInputField.text = "1";
        }

        private void OnButtonClick()
        {
            new DispatcherHandle().Dispatch(new ChangeCountCommand { Delta = int.Parse(uInputField.text) });
        }
    }
}