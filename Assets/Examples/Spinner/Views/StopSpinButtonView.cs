using AIR.Fluxity;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.Spinner
{
    public class StopSpinButtonView : MonoBehaviour
    {
        [SerializeField] private Text uButtonText;
        [SerializeField] private Button uButton;
        private DispatcherHandle _dispatcherHandle;
        public void Start()
        {
            _dispatcherHandle = new DispatcherHandle();
            uButtonText.text = "Stop";
            uButton.onClick.AddListener(OnClick);
        }

        public void OnDestroy() => uButton.onClick.RemoveListener(OnClick);
        private void OnClick()
        {
            _dispatcherHandle.Dispatch(new StopSpinCommand());
        }
    }
}