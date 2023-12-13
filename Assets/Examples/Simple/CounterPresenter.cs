using AIR.Fluxity;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.Simple
{
    public class CounterPresenter : Presenter
    {
        [SerializeField] private Text uLabelText;
        [SerializeField] private Text uCountText;

        private IFeatureView<CounterState> _counterStateBinding;

        public override void CreateBindings()
        {
            _counterStateBinding = Bind<CounterState>();
        }

        public override void Display()
        {
            uCountText.text = _counterStateBinding.State.CurrentCount.ToString();
        }

        protected override void SetUp()
        {
            uLabelText.text = "Current Count:";
        }
    }
}