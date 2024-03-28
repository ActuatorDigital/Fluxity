using AIR.Fluxity;
using Examples.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.DataCommand
{
    public class CounterView : MonoBehaviour
    {
        public struct Model
        {
            public int CurrentCount;
        }

        [SerializeField] private Text uLabelText;
        [SerializeField] private Text uCountText;
        [SerializeField] private InputField uInputField;
        [SerializeField] private ButtonView uButtonView;

        public void Start()
        {
            SetUp();
        }

        public void UpdateModel(Model model)
        {
            uCountText.text = model.CurrentCount.ToString();
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