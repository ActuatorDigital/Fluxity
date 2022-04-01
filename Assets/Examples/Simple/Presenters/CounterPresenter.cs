using AIR.Fluxity;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.Counter
{
    public class CounterPresenter : Presenter
    {
        [SerializeField] private Text uLabelText;
        [SerializeField] private Text uCountText;

        private IFeaturePresenterBinding<CounterState> _counterStateBinding;

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
            uLabelText.text = "Current Count:";
        }
    }
}