using AIR.Fluxity;
using Examples.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.DataCommand
{
    public class CounterPresenter : Presenter
    {
        [SerializeField] private Text uLabelText;
        [SerializeField] private Text uCountText;
        [SerializeField] private InputField uInputField;
        [SerializeField] private ButtonView uButtonView;

        private IFeaturePresenterBinding<CounterState> _counterStateBinding;
        private DispatcherHandle _dispatcherHandle;

        public override void CreateBindings()
        {
            _counterStateBinding = Bind<CounterState>();
        }

        public override void Display()
        {
            uCountText.text = _counterStateBinding.CurrentState.CurrentCount.ToString();
        }

        protected override void SetUp()
        {
            _dispatcherHandle = new DispatcherHandle();
            uLabelText.text = "Current Count:";
            uButtonView.SetButtonText("Change Count");
            uButtonView.SetOnClickedCallback(OnButtonClick);
            uInputField.contentType = InputField.ContentType.IntegerNumber;
            uInputField.text = "1";
        }

        private void OnButtonClick()
        {
            var command = new ChangeCountCommand { Delta = int.Parse(uInputField.text) };
            _dispatcherHandle.Dispatch(command);
        }
    }
}