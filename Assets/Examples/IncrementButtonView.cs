using AIR.Flume;
using AIR.Fluxity;
using UnityEngine;
using UnityEngine.UI;

public class IncrementButtonView : DependentBehaviour
{
    [SerializeField] private Button _button;

    private IDispatcher _dispatcher;

    public void Inject(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        
        _button.onClick.AddListener(() => _dispatcher.Dispatch(new IncrementCountCommand()));
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
