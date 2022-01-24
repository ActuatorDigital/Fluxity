using AIR.Fluxity;
using UnityEngine;

public class IncrementButtonPresenter : DispatchingPresenter
{
    [SerializeField] private IncrementButtonView _incrementButtonView;

    public override void CreateBindings() { }

    public override void Display()
    {
        _incrementButtonView.SetButtonText("Increment");
        _incrementButtonView.SetOnClickedCallback(() => Dispatch(new IncrementCountCommand()));
    }
}
