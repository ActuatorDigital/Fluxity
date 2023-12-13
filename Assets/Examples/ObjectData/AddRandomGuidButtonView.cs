using System;
using System.Linq;
using AIR.Fluxity;
using Examples.Shared;
using UnityEngine;

namespace Examples.ObjectData
{
    public class AddRandomGuidButtonView : MonoBehaviour
    {
        [SerializeField] private ButtonView uButtonView;

        public void Start()
        {
            var dispatcherHandle = new DispatcherHandle();
            uButtonView.SetButtonText("Add");
            uButtonView.SetOnClickedCallback(() => dispatcherHandle.Dispatch(new AddObjectDataCommand { GuidsToAdd = Enumerable.Range(0, 3).Select(_ => Guid.NewGuid()).ToArray() }));
        }
    }
}