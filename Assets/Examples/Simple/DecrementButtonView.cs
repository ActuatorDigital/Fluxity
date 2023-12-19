using AIR.Fluxity;
using Examples.Shared;
using UnityEngine;

namespace Examples.Simple
{
    // NOTE: Decrement/IncrementButtonView are near-duplicates just to make the example clearer.
    // A cleaner view would take in a text to display and a command to dispatch.
    public class DecrementButtonView : MonoBehaviour
    {
        [SerializeField] private ButtonView uButtonView;

        public void Start()
        {
            uButtonView.SetButtonText("Decrease");
            uButtonView.SetOnClickedCallback(() => new DispatcherHandle().Dispatch(new DecrementCountCommand()));
        }
    }
}