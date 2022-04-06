using AIR.Fluxity;
using Examples.Shared;
using UnityEngine;

namespace Examples.Simple
{
    public class DecrementButtonView : MonoBehaviour
    {
        [SerializeField] private ButtonView uButtonView;

        private DispatcherHandle _dispatcherHandle;

        public void Start()
        {
            _dispatcherHandle = new DispatcherHandle();
            uButtonView.SetButtonText("Decrease");
            uButtonView.SetOnClickedCallback(() => _dispatcherHandle.Dispatch(new DecrementCountCommand()));
        }
    }
}