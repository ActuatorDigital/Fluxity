using AIR.Fluxity;
using Examples.Shared;
using UnityEngine;

namespace Examples.Counter
{
    public class IncrementButtonPresenter : DispatchingPresenter
    {
        [SerializeField] private ButtonView uButtonView;

        public override void CreateBindings() { }

        public override void Display()
        {
            uButtonView.SetButtonText("Increase");
            uButtonView.SetOnClickedCallback(() => Dispatch(new IncrementCountCommand()));
        }
    }
}