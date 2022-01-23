using AIR.Fluxity;
using UnityEngine;
using UnityEngine.UI;

public class CounterPresenter : Presenter
{
    [SerializeField] private Text _text;
    private IFeaturePresenterBinding<CounterState> _counterStateBinding;
    
    public override void CreateBindings()
    {
        _counterStateBinding = Bind<CounterState>();
    }

    public override void Display()
    {
        _text.text = _counterStateBinding.CurrentState.CurrentCount.ToString();
    }
}
