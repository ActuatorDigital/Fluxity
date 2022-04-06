using AIR.Fluxity;
using Examples.Shared;
using UnityEngine;

namespace Examples.Simple
{
    public class IncrementButtonView : MonoBehaviour
    {
        [SerializeField] private ButtonView uButtonView;

        private DispatcherHandle _dispatcherHandle;

        public void Start()
        {
            _dispatcherHandle = new DispatcherHandle();
            uButtonView.SetButtonText("Increase");
            uButtonView.SetOnClickedCallback(() => _dispatcherHandle.Dispatch(new IncrementCountCommand()));
        }
    }
}