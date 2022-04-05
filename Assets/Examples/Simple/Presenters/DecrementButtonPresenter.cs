using AIR.Fluxity;
using Examples.Shared;
using UnityEngine;

namespace Examples.Simple
{
    public class DecrementButtonPresenter : DispatchingPresenter
    {
        [SerializeField] private ButtonView uButtonView;

        public override void CreateBindings() { }

        public override void Display()
        {
            uButtonView.SetButtonText("Decrease");
            uButtonView.SetOnClickedCallback(() => Dispatch(new DecrementCountCommand()));
        }
    }
}